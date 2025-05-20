using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Common.Dtos.Requests;

public class AnyValidMoveRequeset
{
  public required GameDto Game { get; set; }
  public required int Roll { get; set; }
}
