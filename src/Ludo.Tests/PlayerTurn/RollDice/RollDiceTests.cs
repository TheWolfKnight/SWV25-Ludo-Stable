using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;

namespace Ludo.Tests.PlayerTurn.RollDice;

public class RollDiceTests
{
  [Fact]
  public void RollD6_OnDieRoll_RollDie()
  {
      // Arrange
      DieBase die = new DieD6();
        
      // Act
      int rolled = die.Roll();
        
      // Assert
      rolled.Should().BeGreaterThanOrEqualTo(1).And.BeLessThanOrEqualTo(6);
  }
    
  [Fact]
  public void RollDie_OnPlayerTurn_RollDie()
  {
      // Arrange
      Player player = new Player()
      {
          PlayerNr = 1,
          InPlay = true,
          Home = A.Fake<Home>(),
          Pieces = []
      };
        
      GameOrchestrator orchestrator = new()
      {
          Players = [A.Fake<Player>(), player],
          CurrentPlayer = 1,
          Board = A.Fake<Board>(),
          Die = new DieD6()
      };

      // Act
      bool playerTurn = orchestrator.CurrentPlayer == player.PlayerNr;
      int rolledNo = orchestrator.Die.Roll();

      // Assert
      using AssertionScope scope = new();
      playerTurn.Should().BeTrue();
      rolledNo.Should().BeGreaterThanOrEqualTo(1).And.BeLessThanOrEqualTo(6);
  }
}