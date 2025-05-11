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
    if (tile is not MovementTile movementTile)
      return dto.Game;

    Piece? toMove = movementTile.Pieces.FirstOrDefault();

    if (toMove is null)
      return dto.Game;

    movementTile.MovePiece(toMove, dto.Roll);

    GameDto result = _boardService.CompressBoardToDto(go);
    return result;
  }

  public bool PeekPieceMove(CheckValidMoveRequestDto dto)
  {
    GameOrchestrator go = _boardService.GenerateBoard(dto.Game);

    TileBase tile = go.Board.Tiles[dto.PiecePosition];
    if (tile is not MovementTile movementTile)
      return false;

    Piece? toMove = movementTile.Pieces.FirstOrDefault();

    if (toMove is null)
      return false;

    bool couldMakeMove = movementTile.PeekMove(toMove, dto.Roll);

    return couldMakeMove;
  }
}

