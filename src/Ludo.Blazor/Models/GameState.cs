using Ludo.Blazor.Features.Factory;
using Ludo.Common.Dtos;
using Ludo.Common.Enums;
using Ludo.Common.Models;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;

namespace Ludo.Blazor.Models;

public class GameState
{

  public required Board Board { get; set; }
  public required DieBase Die { get; set; }
  public required List<Player> Players { get; set; }
  public required Player CurrentPlayer { get; set; }

  public void SetNextPlayerByNr(byte playerNr)
  {
    CurrentPlayer = Players.First(player => player.PlayerNr == playerNr);
  }

  public GameDto ToDto()
  {
    GameDto game = new GameDto()
    {
      Version = 1,
      X = Board.X,
      Y = Board.Y,
      CurrentPlayer = CurrentPlayer.PlayerNr,
      Tiles = Board.Tiles.Select(TileDto.FromTile).ToArray(),
      Players = Players.Select(PlayerDto.FromPlayer).ToArray(),
      Die = new DieDto()
      {
        DieType = Die.GetType().FullName ?? "Unknown",
        CurrentRoll = Die.PeekRoll()
      },
    };

    return game;
  }

  public static GameState FromDto(GameDto dto, DieFactory dieFactory)
  {
    Board board = GetBoard(dto.Tiles, dto.X, dto.Y);
    Player[] players = GetPlayers(dto.Players, board);
    DieBase die = dieFactory.GetRolledDie(dto.Die);

    return new GameState
    {
      Board = board,
      Players = players.ToList(),
      Die = die,
      CurrentPlayer = players.First(p => p.PlayerNr == dto.CurrentPlayer)
    };
  }

  private static Board GetBoard(TileDto[] tiles, int x, int y)
  {
    Board board = new()
    {
      X = x,
      Y = y,
      Tiles = tiles.Select(TileBase (_) => null!).ToArray()
    };

    for (int i = 0; i < board.Tiles.Length; i++)
    {
      TileDto tile = tiles[i];
      TileBase converted = TileBase.FromDto(tile, board, tiles);
      board.Tiles[i] = converted;
    }

    for (int i = 0; i < board.Tiles.Length; i++)
    {
      TileBase tileBase = board.Tiles[i];
      TileDto tileDto = tiles[i];

      if (tileBase is MovementTile movementTile)
      {
        try
        {
          movementTile.BindTiles(tileDto, board);
        }
        catch (Exception e)
        {
          throw new InvalidOperationException($"could not handled tile at index {i} due to internal error: \"{e.Message}\"");
        }
      }
    }

    return board;
  }

  private static Player[] GetPlayers(PlayerDto[] players, Board board)
  {
    Player[] playerList = new Player[players.Length];

    for (int i = 0; i < players.Length; i++)
    {
      Player player = null!;

      Piece[] pieces = players[i].PieceLocation.Select(i =>
      {
        Piece piece = new Piece()
        {
          CurrentTile = (board.Tiles[i] as MovementTile)!,
          Owner = player,
          PieceState = board.Tiles[i] switch
          {
            GoalTile => PieceState.InGoal,
            HomeTile => PieceState.Home,
            _ => PieceState.OnBoard,
          }
        };

        piece.CurrentTile.Pieces.Add(piece);

        return piece;
      }).ToArray();

      Home playerHome = new Home()
      {
        HomeTiles = players[i].HomeTiles.Select(i => (board.Tiles[i] as HomeTile)!).ToArray(),
        Owner = player,
      };

      player = new()
      {
        PlayerNr = players[i].PlayerNr,
        InPlay = players[i].InPlay,
        Home = playerHome,
        Pieces = pieces
      };

      playerList[i] = player;
    }

    return playerList;
  }
}
