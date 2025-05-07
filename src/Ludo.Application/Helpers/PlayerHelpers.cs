using Ludo.Common.Dtos;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;

namespace Ludo.Application.Helpers;

public static class PlayerHelpers
{
  public static Player[] ToPlayerNrModels(this PlayerDto[] dtos)
  {
    IEnumerable<Player> players = dtos
      .Select(dto => new Player
      {
        PlayerNr = dto.PlayerNr,
        Pieces = [],
        InPlay = dto.InPlay,
        Home = null!,
      });

    return players.ToArray();
  }

  public static Player[] ToPlayerModels(this PlayerDto[] dtos)
  {
    IEnumerable<Player> players = dtos
      .Select(dto => {
        Home home = null!;
        Player player = new()
        {
          PlayerNr = dto.PlayerNr,
          Pieces = Enumerable //NOTE: prime an array for the piece later, when we have aboard
            .Range(0, dto.PieceLocation.Count())
            .Select(_ => null as Piece!)
            .ToArray()!,
          InPlay = dto.InPlay,
          Home = home,
        };

        home = new()
        {
          HomeTiles = Enumerable
            .Range(0, dto.HomeTiles.Count())
            .Select(_ => null as HomeTile!)
            .ToArray()!,
          Owner = player,
        };

        return player;
      });

    return players.ToArray();
  }
}
