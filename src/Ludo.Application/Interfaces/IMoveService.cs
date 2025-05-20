using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Application.Interfaces;

public interface IMoveService
{
  GameDto MovePiece(MakeMoveRequestDto dto);
  bool PeekPieceMove(CheckValidMoveRequestDto dto);
}
