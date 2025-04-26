using Ludo.Common.Models.Dice;

namespace Ludo.Application.Factories;

public class DieFactory
{
  private readonly IEnumerable<DieBase> _dics;

  public DieFactory(IEnumerable<DieBase> dics)
  {
    _dics = dics;
  }

  public DieBase GetDie(string key)
  {
    DieBase die = this._dics
      .FirstOrDefault(die => die.GetType().FullName == key, new DieD6());

    return die;
  }
}

