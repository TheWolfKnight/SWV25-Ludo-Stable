using System;

namespace Ludo.Common.Models;

public class DieBase
{
  public required virtual int[] Faces { get; set; } 

  public virtual int Roll()
  {
    throw new NotImplementedException();
  }

  public virtual int PeekRoll()
  {
    throw new NotImplementedException();
  }
}
