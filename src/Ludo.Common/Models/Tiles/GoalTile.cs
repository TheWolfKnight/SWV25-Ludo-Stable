using Ludo.Common.Dtos;
using Ludo.Common.Models.Player;
using Ludo.Common.Interfaces.Tiles;
using Ludo.Common.Enums;

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
  
  internal new static GoalTile FromDto(TileDto tileDto, Board board)
  {
    int previousTileIndex = (int) (tileDto.Data[nameof(PreviousTile)] ?? throw new InvalidCastException("Could not get PreviousTile index"));
    DriveWayTile previousTile = board.Tiles[previousTileIndex] as DriveWayTile ?? throw new InvalidCastException("Could not convert tile to DriveWayTile");

    GoalTile tile = new()
    {
      PreviousTile = previousTile,
      PlayerNr = (byte?)tileDto.Data[nameof(PlayerNr)],
      IndexInBoard = (int)(tileDto.Data[nameof(IndexInBoard)] ?? throw new InvalidCastException("Could not convert to Index on board")),
      Pieces = []
    };

    return tile;
  }
}
