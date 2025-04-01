using System;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models;

public class GameOrchestrator
{
  public required byte CurrentPlayer { get; set; }
  public required DieBase Die { get; init; }
  public required Board Board { get; init; }
  public required Player.Player[] Players { get; set; }

  public void StartGame()
  {
    throw new NotImplementedException();
  }

  public void EndGame()
  {
    throw new NotImplementedException();
  }

  public void RestartGame()
  {
    throw new NotImplementedException();
  }

  public virtual void NextPlayer()
  {
    CurrentPlayer = (byte)((CurrentPlayer + 1) % Players.Length);
  }

  public bool IsValidMove(Piece piece)
  {
    throw new NotImplementedException();
  }
}
