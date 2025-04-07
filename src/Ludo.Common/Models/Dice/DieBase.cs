namespace Ludo.Common.Models.Dice;

public abstract class DieBase
{
  protected readonly Random Random = new();
  protected int CurrentInt = 0;
  
  public abstract int[] Faces { get; set; }

  public abstract int Roll();

  public abstract int PeekRoll();
}
