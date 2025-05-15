using Ludo.Application.Factories;
using Ludo.Common.Dtos;
using Ludo.Common.Enums;
using Ludo.Common.Models;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;

namespace Ludo.Application.Services;

public class BoardGenerationService
{
  private readonly DieFactory _dieFactory;

  public BoardGenerationService(DieFactory dieFactory)
  {
    _dieFactory = dieFactory;
  }
  
  public GameOrchestrator GenerateBoard(GameDto dto)
  {
    Board board = GetBoard(dto.Tiles);
    Player[] players = GetPlayers(dto.Players, board);

    GameOrchestrator orchestrator = new()
    {
      Players = players,
      Board = board,
      CurrentPlayer = dto.CurrentPlayer,
      Die = _dieFactory.GetRolledDie(dto.Die)
    };

    return orchestrator;
  }

  public GameDto CompressBoardToDto(GameOrchestrator go)
  {
    GameDto game = new GameDto()
    {
      Version = 1,
      X = 15,
      Y = 15,
      CurrentPlayer = go.CurrentPlayer,
      Tiles = go.Board.Tiles.Select(TileDto.FromTile).ToArray(),
      Players = go.Players.Select(PlayerDto.FromPlayer).ToArray(),
      Die = new DieDto()
      {
        DieType = go.Die.GetType().FullName ?? "Unknown",
        CurrentRoll = go.Die.PeekRoll()
      },
    };

    return game;
  }

  private Board GetBoard(TileDto[] tiles)
  {
    Board board = new()
    {
      Tiles = tiles.Select(TileBase (_) => null!).ToArray()
    };

    for(int i = 0; i < board.Tiles.Length; i++)
    {
      TileDto tile = tiles[i];
      TileBase converted = TileBase.FromDto(tile, board);
      board.Tiles[i] = converted;
    }

    return board;
  }

  private Player[] GetPlayers(PlayerDto[] players, Board board)
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
        Pieces = new()
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
