using Ludo.Common.Dtos;
using Ludo.Common.Enums;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public abstract class TileBase
{
  public virtual byte? PlayerNr { get; init; }
  public required int IndexInBoard { get; set; }
  public required List<Piece> Pieces { get; set; }

  public abstract void MovePiece(Piece piece, int amount);
  public abstract bool PeekMove(Piece piece, int amount); 

  internal abstract (bool MoveAccepted, TileBase TargetTile) InternalMakeMove(Piece piece, int amount);
  internal abstract void TakePiece(Piece piece);

  public static TileBase FromDto(TileDto tileDto, Board board)
  {
    return tileDto.Type switch
    {
      TileTypes.Standard => StandardTile.FromDto(tileDto, board),
      TileTypes.Home => HomeTile.FromDto(tileDto, board),
      TileTypes.DriveWay => DriveWayTile.FromDto(tileDto, board),
      TileTypes.Filter => FilterTile.FromDto(tileDto, board),
      TileTypes.Goal => GoalTile.FromDto(tileDto, board),
      _ => throw new InvalidOperationException($"Unknown TileType : {tileDto.Type}")
    };
  }
}
