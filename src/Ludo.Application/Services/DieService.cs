using Ludo.Application.Factories;
using Ludo.Common.Models.Dice;
using Ludo.Common.Dtos;

namespace Ludo.Application.Services;

public class DieService
{
  private readonly DieFactory _factory;

  public DieService(DieFactory factory)
  {
    _factory = factory;
  }

  public int RollDie(DieDto dieDto)
  {
    DieBase die = _factory.GetDie(dieDto.DieType);

    int roll = die.Roll();
    return roll;
  }

  public int PeekDieRoll(DieDto dieDto)
  {
    int result = 0;
    if (dieDto.CurrentRoll.HasValue)
      result = dieDto.CurrentRoll.Value;
    return result;
  }
}
