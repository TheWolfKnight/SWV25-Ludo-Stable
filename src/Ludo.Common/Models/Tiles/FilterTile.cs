using Ludo.Common.Dtos;
using Ludo.Common.Enums;
using Ludo.Common.Models.Player;
using System.Text.Json;

namespace Ludo.Common.Models.Tiles;

public class FilterTile : MovementTile
{
  public required MovementTile NextTile { get; set; }
  public required DriveWayTile FilterdTile { get; set; }

  public override void MovePiece(Piece piece, int amount)
  {
    (bool moveAccepted, MovementTile targetTile) = InternalMakeMove(piece, amount);
    if (!moveAccepted)
      return;

    base.Pieces.Remove(piece);

    int opponentCount = targetTile.Pieces.Count(inner => inner.Owner.PlayerNr != piece.Owner.PlayerNr);
    if (opponentCount > 1)
    {
      piece.MoveToHome();
      return;
    }
    else if (opponentCount == 1)
    {
      Piece opp = targetTile.Pieces.First();
      opp.MoveToHome();
    }

    targetTile.TakePiece(piece);
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    return InternalMakeMove(piece, amount).MoveAccepted;
  }

  public override void BindTiles(TileDto tileDto, Board board)
  {
    int nextIndex = ((JsonElement?)tileDto.Data[nameof(NextTile)])?.Deserialize<int>() ?? throw new InvalidOperationException("Cannot find NextTile for FilterTile");

    MovementTile next = board.Tiles[nextIndex] as MovementTile ?? throw new InvalidCastException($"Cannot cast tile at index {nextIndex} as a MovementTile");
    NextTile = next;

    int filterdIndex = ((JsonElement?)tileDto.Data[nameof(FilterdTile)])?.Deserialize<int>() ?? throw new InvalidOperationException("Cannot find FilterdTile for FilterTile");

    DriveWayTile filterdTile = board.Tiles[filterdIndex] as DriveWayTile ?? throw new InvalidCastException($"Cannot cast tile at index {nextIndex} as a DriveWayTile");
    FilterdTile = filterdTile;
  }

  internal override (bool MoveAccepted, MovementTile TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    if (amount is 0)
      return (true, this);

    bool isFilterdPlayer = base.PlayerNr == piece.Owner.PlayerNr;
    MovementTile targetTile = isFilterdPlayer
      ? FilterdTile
      : NextTile;

    bool containsOwnPiece = targetTile.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr);
    if (amount is <= 1 && containsOwnPiece)
      return (false, this);

    return targetTile.InternalMakeMove(piece, amount - 1);
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.OnBoard;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
  
  internal new static FilterTile FromDto(TileDto tileDto, Board board, TileDto[] tiles)
  {
    int nextTileIndex = ((JsonElement?)tileDto.Data[nameof(NextTile)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not get NextTile index");
    TileDto nextTile = tiles[nextTileIndex];

    if (nextTile.Type is TileTypes.Filler)
      throw new InvalidOperationException("Cannot bind to non-MovementTile");

    int filteredTileIndex = ((JsonElement?)tileDto.Data[nameof(FilterdTile)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not get FilteredTile index");
    if (tiles[filteredTileIndex].Type is not TileTypes.DriveWay)
      throw new InvalidCastException("Could not convert tile to DriveWayTile");

    int? playerNr = ((JsonElement?)tileDto.Data[nameof(PlayerNr)])?.Deserialize<int>();
    if (playerNr == -1)
      playerNr = null;

    FilterTile tile = new()
    {
      NextTile = (board.Tiles[nextTileIndex] as MovementTile)!,
      FilterdTile = (board.Tiles[filteredTileIndex] as DriveWayTile)!,
      PlayerNr = (byte?)playerNr,
      IndexInBoard = ((JsonElement?)tileDto.Data[nameof(IndexInBoard)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not convert to Index on board"),
      Pieces = []
    };

    return tile;
  }
}
