using Ludo.Common.Dtos;
using Ludo.Common.Models.Player;
using Ludo.Common.Interfaces.Tiles;
using Ludo.Common.Enums;
using System.Text.Json;

namespace Ludo.Common.Models.Tiles;

public class GoalTile : MovementTile, IGoalTile
{
  public override required byte? PlayerNr { get; init; }
  public required DriveWayTile PreviousTile { get; set; }

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
    int index = ((JsonElement?)tileDto.Data[nameof(PreviousTile)])?.Deserialize<int>() ?? throw new InvalidOperationException("Cannot find PreviousTile for GoalTile");

    DriveWayTile prev = board.Tiles[index] as DriveWayTile ?? throw new InvalidCastException($"Cannot cast tile at index {index} as a DriveWayTile");
    PreviousTile = prev;
  }

  internal override (bool MoveAccepted, MovementTile TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    if (amount is 0)
      return (true, this);

    return PreviousTile.DriveWayMakeMove(piece, amount - 1, false);
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.InGoal;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
  
  internal new static GoalTile FromDto(TileDto tileDto, Board board, TileDto[] tiles)
  {
    int previousTileIndex = ((JsonElement?)tileDto.Data[nameof(PreviousTile)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not get PreviousTile index");
    if (tiles[previousTileIndex].Type is not TileTypes.DriveWay)
      throw new InvalidCastException("Could not convert tile to DriveWayTile");

    int? playerNr = ((JsonElement?)tileDto.Data[nameof(PlayerNr)])?.Deserialize<int>();

    GoalTile tile = new()
    {
      PreviousTile = (board.Tiles[previousTileIndex] as DriveWayTile)!,
      PlayerNr = (byte?)playerNr,
      IndexInBoard = ((JsonElement?)tileDto.Data[nameof(IndexInBoard)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not convert to Index on board"),
      Pieces = []
    };

    return tile;
  }
}
