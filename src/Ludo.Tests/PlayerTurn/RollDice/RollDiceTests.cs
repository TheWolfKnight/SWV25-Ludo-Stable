using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models;
using Ludo.Common.Models.Player;

namespace Ludo.Tests.PlayerTurn.RollDice;

public class RollDiceTests
{
    /// <summary>
    /// Missing methods/states?
    /// </summary>
    [Fact]
    public void RollDie_OnMyTurn_RollDie()
    {
        // Arrange
        Player player = new Player()
        {
            PlayerNr = 1,
            InPlay = true,
            Home = A.Fake<Home>(),
            Pieces = []
        };
        
        GameOrchestrator orchestrator = new GameOrchestrator()
        {
            Players = [A.Fake<Player>(), player],
            CurrentPlayer = 0,
            Board = A.Fake<Board>(),
            Die = A.Fake<DieBase>()
        };

        // Act
        orchestrator.NextPlayer();
        bool playerTurn = orchestrator.CurrentPlayer == player.PlayerNr;
        int? rolled = orchestrator.Die.Roll();

        // Assert
        using AssertionScope scope = new();
        playerTurn.Should().BeTrue();
        rolled.Should().BeGreaterThan(0);
    }

    [Fact]
    public void RollDie_NotOnMyTurn_NotRollDie()
    {
        // Arrange
        Player player = new Player()
        {
            PlayerNr = 0,
            InPlay = true,
            Home = A.Fake<Home>(),
            Pieces = []
        };
        
        GameOrchestrator orchestrator = new GameOrchestrator()
        {
            Players = [player, A.Fake<Player>()],
            CurrentPlayer = 0,
            Board = A.Fake<Board>(),
            Die = A.Fake<DieBase>()
        };
        
        // Act
        orchestrator.NextPlayer();
        bool playerTurn = orchestrator.CurrentPlayer == player.PlayerNr;
        
        // Assert
        playerTurn.Should().BeFalse();
    }
}