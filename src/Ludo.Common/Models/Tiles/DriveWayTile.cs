using Ludo.Common.Dtos;
using Ludo.Common.Enums;
using Ludo.Common.Interfaces.Tiles;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class DriveWayTile : TileBase, IGoalTile
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

    (bool moveAccepted, TileBase targetTile) = InternalMakeMove(piece, amount);

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

  internal override (bool MoveAccepted, TileBase TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    return DriveWayMakeMove(piece, amount, true);
  }

  public (bool MoveAccepted, TileBase TargetTile) DriveWayMakeMove(Piece piece, int amount, bool goForward)
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

    (bool, TileBase) result;
    if (nextTile is DriveWayTile)
      result = ((DriveWayTile)nextTile).DriveWayMakeMove(piece, amount - 1, goForward);
    else
      result = ((TileBase)nextTile).InternalMakeMove(piece, amount - 1);

    return result;
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.OnBoard;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
  
  internal new static DriveWayTile FromDto(TileDto tileDto, Board board)
  {
    int nextTileIndex = (int) (tileDto.Data[nameof(NextTile)] ?? throw new InvalidCastException("Could not get NextTile index"));
    IGoalTile nextTile = board.Tiles[nextTileIndex] as IGoalTile ?? throw new InvalidCastException("Could not convert tile to IGoalTile");
    
    int previousTileIndex = (int) (tileDto.Data[nameof(PreviousTile)] ?? throw new InvalidCastException("Could not get PreviousTile index"));
    DriveWayTile previousTile = board.Tiles[previousTileIndex] as DriveWayTile ?? throw new InvalidCastException("Could not convert tile to DriveWayTile");
    
    DriveWayTile tile = new()
    {
      PreviousTile = previousTile,
      NextTile = nextTile,
      PlayerNr = (byte?) tileDto.Data[nameof(PlayerNr)],
      IndexInBoard = (int) (tileDto.Data[nameof(IndexInBoard)] ?? throw new InvalidCastException("Could not convert to Index on board")),
      Pieces = [],
    };

    return tile;
  }
}
