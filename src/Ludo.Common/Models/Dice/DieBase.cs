namespace Ludo.Common.Models.Dice;

public abstract class DieBase
{
  protected int CurrentRoll = 0;
  public abstract int[] Faces { get; set; }

  public DieBase()
  {}

  public DieBase(int currentRoll)
  {
    CurrentRoll = currentRoll;
  }

  public abstract int Roll();
  public abstract int PeekRoll();
}
