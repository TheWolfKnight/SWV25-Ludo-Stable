namespace Ludo.Common.Models.Dice;

public abstract class DieBase
{
  private readonly Random _random = new();
  private int _currentInt = 0;
  
  public virtual int[] Faces { get; set; } = [1, 2, 3, 4, 5, 6]; 

  public virtual int Roll()
  {
    return Faces[_random.Next(1, Faces.Length)];
  }

  public virtual int PeekRoll()
  {
    return _currentInt;
  }
}
