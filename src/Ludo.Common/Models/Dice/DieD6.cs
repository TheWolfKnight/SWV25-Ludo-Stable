namespace Ludo.Common.Models.Dice;

public class DieD6 : DieBase
{
  private readonly Random _random;

  public override int[] Faces { get; set; } = [1, 2, 3, 4, 5, 6];

  public DieD6(): base()
  {
    _random = GetSeededRandom();
  }

  public DieD6(int currentRoll): base(currentRoll)
  {
    _random = GetSeededRandom();
  }

  public override int Roll()
  {
    int no = _random.Next(0, Faces.Length);
    
    CurrentRoll = Faces[no];

    return CurrentRoll;
  }

  public override int PeekRoll()
  {
    return CurrentRoll;
  }

  private Random GetSeededRandom()
  {
    DateTime dt = DateTime.UtcNow;
    Random rnd = new((int) dt.Ticks);
    return rnd;
  }
}
