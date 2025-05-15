using Ludo.Common.Dtos;
using Ludo.Common.Models.Player;
using Ludo.Common.Enums;
using System.Text.Json;

namespace Ludo.Common.Models.Tiles;

public class HomeTile: MovementTile
{
  public override required byte? PlayerNr { get; init; }
  public required MovementTile NextTile { get; set; }

  public void SendPieceHome(Piece piece)
  {
    TakePiece(piece);
  }

  public override void MovePiece(Piece piece, int amount)
  {
    (bool moveAccepted, MovementTile targetTile) = InternalMakeMove(piece, amount);

    if (!moveAccepted)
      return;

    int opponentCount = NextTile.Pieces.Count(inner => inner.Owner.PlayerNr != piece.Owner.PlayerNr);
    if (opponentCount > 1)
      return;
    else if (opponentCount == 1)
    {
      Piece opp = targetTile.Pieces.First();
      opp.MoveToHome();
    }

    base.Pieces.Remove(piece);
    targetTile.TakePiece(piece);
  }

  public override bool PeekMove(Piece piece, int amount) 
  {
    return InternalMakeMove(piece, amount).MoveAccepted;
  }

  public override void BindTiles(TileDto tileDto, Board board)
  {
    int index = ((JsonElement?)tileDto.Data[nameof(NextTile)])?.Deserialize<int>() ?? throw new InvalidOperationException("Cannot find NextTile for HomeTile");

    MovementTile next = board.Tiles[index] as MovementTile ?? throw new InvalidCastException($"Cannot cast tile at index {index} as a MovementTile");
    NextTile = next;
  }

  internal override (bool MoveAccepted, MovementTile TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    (bool, MovementTile) result = (true, NextTile);
    if (amount is not 6)
      result = (false, this);

    return result;
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.Home;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
  
  internal new static HomeTile FromDto(TileDto tileDto, Board board, TileDto[] tiles)
  {
    int nextTileIndex = ((JsonElement?)tileDto.Data[nameof(NextTile)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not get NextTile index");
    TileDto nextTile = tiles[nextTileIndex];

    if (nextTile.Type is TileTypes.Filler)
      throw new InvalidOperationException("Cannot bind to non-MovementTile");

    int? playerNr = ((JsonElement?)tileDto.Data[nameof(PlayerNr)])?.Deserialize<int>();
    if (playerNr == -1)
      playerNr = null;

    HomeTile tile = new()
    {
      NextTile = (board.Tiles[nextTileIndex] as MovementTile)!,
      PlayerNr = (byte?)playerNr,
      IndexInBoard = ((JsonElement?)tileDto.Data[nameof(IndexInBoard)])?.Deserialize<int>() ?? throw new InvalidCastException("Could not convert to Index on board"),
      Pieces = [],
    };

    return tile;
  }
}
