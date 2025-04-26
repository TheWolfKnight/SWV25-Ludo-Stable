using Ludo.Common.Dtos.Requests;
using Ludo.Common.Dtos;
using Ludo.Common.Models;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Models.Player;

namespace Ludo.Application.Services;

public class MoveService
{
  private readonly BoardGenerationService _boardService;

  public MoveService(BoardGenerationService boardService)
  {
    _boardService = boardService;
  }

  public GameDto MovePiece(MakeMoveRequestDto dto)
  {
    GameOrchestrator go = _boardService.GenerateBoard(dto.Game);

    TileBase tile = go.Board.Tiles[dto.PiecePosition];
    Piece? toMove = tile.Pieces.FirstOrDefault();

    if (toMove is null)
      return dto.Game;

    tile.MovePiece(toMove, dto.Roll);

    GameDto result = _boardService.ComperssBoardToDto(go);
    return result;
  }

  public bool PeekPieceMove(CheckValidMoveRequestDto dto)
  {
    GameOrchestrator go = _boardService.GenerateBoard(dto.Game);

    TileBase tile = go.Board.Tiles[dto.PiecePosition];
    Piece? toMove = tile.Pieces.FirstOrDefault();

    if (toMove is null)
      return false;

    bool couldMakeMove = tile.PeekMove(toMove, dto.Roll);

    return couldMakeMove;
  }
}

