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

  internal override (bool MoveAccepted, MovementTile TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    if (amount is 0)
      return (true, this);

    bool containsOwnPiece = base.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr && inner != piece);
    if (containsOwnPiece)
      return (false, this);

    bool isFilterdPlayer = base.PlayerNr == piece.Owner.PlayerNr;
    Func<Piece, int, (bool MoveAccepted, MovementTile TargetTile)> method = isFilterdPlayer
      ? FilterdTile.InternalMakeMove
      : NextTile.InternalMakeMove;

    return method(piece, amount - 1);
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.OnBoard;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
  
  internal new static FilterTile FromDto(TileDto tileDto, Board board)
  {
    int nextTileIndex = ((JsonElement?)tileDto.Data[nameof(NextTile)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not get NextTile index");
    TileBase nextTile = board.Tiles[nextTileIndex];

    if (nextTile is not MovementTile or null)
      throw new InvalidOperationException("Cannot bind to non-MovementTile");

    int filteredTileIndex = ((JsonElement?)tileDto.Data[nameof(FilterdTile)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not get FilteredTile index");
    DriveWayTile filteredTile = board.Tiles[filteredTileIndex] as DriveWayTile ?? throw new InvalidCastException("Could not convert tile to DriveWayTile");

    int? playerNr = ((JsonElement?)tileDto.Data[nameof(PlayerNr)])?.Deserialize<int>();
    if (playerNr == -1)
      playerNr = null;

    FilterTile tile = new()
    {
      NextTile = (nextTile as MovementTile)!,
      FilterdTile = filteredTile,
      PlayerNr = (byte?)playerNr,
      IndexInBoard = ((JsonElement?)tileDto.Data[nameof(IndexInBoard)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not convert to Index on board"),
      Pieces = []
    };

    return tile;
  }
}
