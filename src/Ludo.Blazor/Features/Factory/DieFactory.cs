using Ludo.Common.Dtos;
using Ludo.Common.Models.Dice;

namespace Ludo.Blazor.Features.Factory;

public class DieFactory
{
  private readonly IEnumerable<DieBase> _dice;

  public DieFactory(IEnumerable<DieBase> dice)
  {
    _dice = dice;
  }

  public DieBase GetDie(string key)
  {
    DieBase die = this._dice
      .FirstOrDefault(die => die.GetType().FullName == key, new DieD6());

    return die;
  }

  public DieBase GetRolledDie(DieDto dto)
  {
    DieBase die = GetDie(dto.DieType);

    DieBase? dieBase = die
      .GetType()
      .GetConstructor([typeof(int)])?
      .Invoke([dto.CurrentRoll]) as DieBase;

    return dieBase ?? die;
  }
}

