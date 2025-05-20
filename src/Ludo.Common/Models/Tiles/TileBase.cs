using Ludo.Common.Dtos;
using Ludo.Common.Enums;

namespace Ludo.Common.Models.Tiles;

public abstract class TileBase
{
  public virtual byte? PlayerNr { get; init; }
  public required int IndexInBoard { get; set; }

  public static TileBase FromDto(TileDto tileDto, Board board, TileDto[] tiles)
  {
    return tileDto.Type switch
    {
      TileTypes.Filler => FillerTile.FromDto(tileDto),
      TileTypes.Standard => StandardTile.FromDto(tileDto, board, tiles),
      TileTypes.Home => HomeTile.FromDto(tileDto, board, tiles),
      TileTypes.DriveWay => DriveWayTile.FromDto(tileDto, board, tiles),
      TileTypes.Filter => FilterTile.FromDto(tileDto, board, tiles),
      TileTypes.Goal => GoalTile.FromDto(tileDto, board, tiles),
      _ => throw new InvalidOperationException($"Unknown TileType : {tileDto.Type}")
    };
  }
}
