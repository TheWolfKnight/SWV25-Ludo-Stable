using Ludo.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Application.Interfaces;

public interface IDieService
{
  int RollDie(DieDto dieDto);
  int PeekDieRoll(DieDto dieDto);
}
