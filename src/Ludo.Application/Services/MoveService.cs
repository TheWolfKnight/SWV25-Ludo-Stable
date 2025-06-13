using Ludo.Common.Dtos.Requests;
using Ludo.Common.Dtos;
using Ludo.Common.Models;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Models.Player;
using Ludo.Application.Interfaces;

namespace Ludo.Application.Services;

public class MoveService: IMoveService
{
  private readonly IBoardGenerationService _boardService;

  public MoveService(IBoardGenerationService boardService)
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
      throw new InvalidOperationException("Could not find piece");

    if (toMove.Owner.PlayerNr != go.CurrentPlayer)
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

