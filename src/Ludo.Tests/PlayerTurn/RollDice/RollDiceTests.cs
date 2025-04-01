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
    
    /// <summary>
    /// Missing methods/states?
    /// </summary>
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
            CurrentPlayer = 0,
            Board = A.Fake<Board>(),
            Die = A.Fake<DieBase>()
        };

        // Act
        bool playerTurn = orchestrator.CurrentPlayer == player.PlayerNr;
        int rolledNo = orchestrator.Die.Roll();

        // Assert
        using AssertionScope scope = new();
        playerTurn.Should().BeTrue();
        rolledNo.Should().BeGreaterThanOrEqualTo(1).And.BeLessThanOrEqualTo(6);
    }

    [Theory]
    [MemberData(nameof(GetGameOrchestratorWithPlayers))]
    public void RollDie_NotOnMyTurn_NotRollDie(GameOrchestrator orchestrator, Player player)
    {
        // Act
        orchestrator.NextPlayer();
        bool playerTurn = orchestrator.CurrentPlayer == player.PlayerNr;
        
        // Assert
        playerTurn.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetGameOrchestratorWithPlayers()
    {
        Player player = new()
        {
            PlayerNr = 1,
            InPlay = true,
            Home = A.Fake<Home>(),
            Pieces = []
        };

        return new[] 
        {
            new object[]
            {
                new GameOrchestrator()
                {
                    Players = [player, A.Fake<Player>()],
                    CurrentPlayer = 0,
                    Board = A.Fake<Board>(),
                    Die = A.Fake<DieBase>()
                },
                player
            }
        };
    }
}