using Ludo.Common.Dtos;
using Ludo.Common.Enums;
using Ludo.Common.Models.Player;
using System.Text.Json;

namespace Ludo.Common.Models.Tiles;

public class StandardTile : MovementTile
{
  public required MovementTile NextTile { get; set; }

  public override void MovePiece(Piece piece, int amount)
  {
    (bool moveAccepted, MovementTile targetTile) = InternalMakeMove(piece, amount);
    if (!moveAccepted)
      return;

    base.Pieces.Remove(piece);

    int cntOpponentPieces = targetTile.Pieces.Count(inner => inner.Owner.PlayerNr != piece.Owner.PlayerNr && inner != piece);
    if (cntOpponentPieces > 1)
    {
      piece.MoveToHome();
      return;
    }
    else if (cntOpponentPieces == 1)
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
    int index = ((JsonElement?)tileDto.Data[nameof(NextTile)])?.Deserialize<int>() ?? throw new InvalidOperationException("Cannot find NextTile for StandardTile");

    MovementTile next = board.Tiles[index] as MovementTile ?? throw new InvalidCastException($"Cannot cast tile at index {index} as a MovementTile");
    NextTile = next;
  }

  internal override (bool MoveAccepted, MovementTile TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    if (amount is 0)
      return (true, this);

    bool containsOwnPieces = NextTile.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr);
    if (amount is not <= 1 && containsOwnPieces)
      return (false, this);

    return NextTile.InternalMakeMove(piece, amount - 1);
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.OnBoard;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }

  internal new static StandardTile FromDto(TileDto tileDto, Board board, TileDto[] tiles)
  {
    int? playerNr = ((JsonElement?)tileDto.Data[nameof(PlayerNr)])?.Deserialize<int>();

    StandardTile tile = new()
    {
      NextTile = null!,
      PlayerNr = (byte?)playerNr,
      IndexInBoard = ((JsonElement?)tileDto.Data[nameof(IndexInBoard)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not convert to Index on board"),
      Pieces = [],
    };

    return tile;
  }
}
