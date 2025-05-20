using Ludo.Common.Dtos;
using Ludo.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Application.Interfaces;

public interface IBoardGenerationService
{
  GameOrchestrator GenerateBoard(GameDto dto);
  GameDto CompressBoardToDto(GameOrchestrator go);
}
