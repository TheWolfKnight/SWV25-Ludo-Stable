using System.Collections.Generic;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Dtos;

public record PlayerDto
{
  public required byte PlayerNr { get; init; }
  public required bool InPlay { get; init; }
  
  public required IEnumerable<int> HomeTiles { get; init; }
  public required IEnumerable<int> PieceLocation  { get; init; }

  public static PlayerDto FromPlayer(Player player)
  {
    return new PlayerDto
    {
      PlayerNr = player.PlayerNr,
      InPlay = player.InPlay,
      HomeTiles = player.Home.HomeTiles.Select(ht => ht.IndexInBoard),
      PieceLocation = player.Pieces.Select(p => p.CurrentTile.IndexInBoard)
    };
  }
}
