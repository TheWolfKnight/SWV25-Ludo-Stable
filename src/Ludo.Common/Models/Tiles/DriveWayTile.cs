using Ludo.Common.Dtos;
using Ludo.Common.Enums;
using Ludo.Common.Interfaces.Tiles;
using Ludo.Common.Models.Player;
using System.Text.Json;

namespace Ludo.Common.Models.Tiles;

public class DriveWayTile : MovementTile, IGoalTile
{
  public required override byte? PlayerNr { get; init; }

  public required IGoalTile NextTile { get; set; }
  public required DriveWayTile? PreviousTile { get; set; }

  public override void MovePiece(Piece piece, int amount)
  {
    if (piece.Owner.PlayerNr != this.PlayerNr)
    {
      piece.MoveToHome();
      return;
    }

    (bool moveAccepted, MovementTile targetTile) = InternalMakeMove(piece, amount);

    if (!moveAccepted)
      return;

    base.Pieces.Remove(piece);
    targetTile.TakePiece(piece);
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    if (piece.Owner.PlayerNr != this.PlayerNr)
      return false;
    return InternalMakeMove(piece, amount).MoveAccepted;
  }

  public override void BindTiles(TileDto tileDto, Board board)
  {
    int nextIndex = ((JsonElement?)tileDto.Data[nameof(NextTile)])?.Deserialize<int>() ?? throw new InvalidOperationException("Cannot find NextTile for DriveWayTile");

    IGoalTile next = board.Tiles[nextIndex] as IGoalTile ?? throw new InvalidCastException($"Cannot cast tile at index {nextIndex} as a IGoalTile");
    NextTile = next;

    int? prevIndex = ((JsonElement?)tileDto.Data[nameof(PreviousTile)])?.Deserialize<int>();

    DriveWayTile? prev = prevIndex.HasValue
      ? board.Tiles[prevIndex.Value] as DriveWayTile ?? throw new InvalidCastException($"Cannot cast tile at index {prevIndex} as a DriveWayTile")
      : null;
    PreviousTile = prev;
  }

  internal override (bool MoveAccepted, MovementTile TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    return DriveWayMakeMove(piece, amount, true);
  }

  public (bool MoveAccepted, MovementTile TargetTile) DriveWayMakeMove(Piece piece, int amount, bool goForward)
  {
    if (amount is 0)
      return (true, this);

    bool containsOwnPiece = base.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr && inner != piece);
    if (containsOwnPiece)
      return (false, this);

    IGoalTile? nextTile = goForward
      ? NextTile
      : PreviousTile;

    if (nextTile is null)
    {
      nextTile = NextTile;
      goForward = true;
    }

    (bool, MovementTile) result;
    if (nextTile is DriveWayTile)
      result = ((DriveWayTile)nextTile).DriveWayMakeMove(piece, amount - 1, goForward);
    else
      result = ((MovementTile)nextTile).InternalMakeMove(piece, amount - 1);

    return result;
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.OnBoard;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
  
  internal new static DriveWayTile FromDto(TileDto tileDto, Board board, TileDto[] tiles)
  {
    int nextTileIndex = ((JsonElement?)tileDto.Data[nameof(NextTile)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not get NextTile index");
    if (tiles[nextTileIndex].Type is not (TileTypes.DriveWay or TileTypes.Goal))
      throw new InvalidCastException("Could not convert tile to IGoalTile");

    int? previousTileIndex = ((JsonElement?)tileDto.Data[nameof(PreviousTile)])?.Deserialize<int?>();
    if (previousTileIndex.HasValue && tiles[previousTileIndex.Value].Type is not (TileTypes.DriveWay))
      throw new InvalidCastException("Could not convert tile to DriveWayTile");

    int? playerNr = ((JsonElement?)tileDto.Data[nameof(PlayerNr)])?.Deserialize<int>();
    if (playerNr == -1)
      playerNr = null;

    DriveWayTile tile = new()
    {
      PreviousTile = previousTileIndex.HasValue
        ? (board.Tiles[previousTileIndex.Value] as DriveWayTile)!
        : null,
      NextTile = (board.Tiles[nextTileIndex] as IGoalTile)!,
      PlayerNr = (byte?)playerNr,
      IndexInBoard = ((JsonElement?)tileDto.Data[nameof(IndexInBoard)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not convert to Index on board"),
      Pieces = [],
    };

    return tile;
  }
}
