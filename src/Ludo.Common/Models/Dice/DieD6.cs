namespace Ludo.Common.Models.Dice;

public class DieD6 : DieBase
{
  private readonly Random _random = new();
  
  public override int[] Faces { get; set; } = [1, 2, 3, 4, 5, 6];
  
  public override int Roll()
  {
    CurrentInt = Faces[_random.Next(1, Faces.Length)];
    
    return CurrentInt;
  }

  public override int PeekRoll()
  {
    return CurrentInt;
  }
}