using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models;
using Ludo.Common.Models.Player;

namespace Ludo.Tests.PlayerTurn.SkipTurn;

public class SkipTurnTests
{
    [Fact]
    public void SkipTurn_NoValidMove_SkipTurn()
    {
        // Arrange
        Player currentPlayer = new Player()
        {
            PlayerNr = 0,
            InPlay = true,
            Home = A.Fake<Home>(),
            Pieces = []
        };
        
        Player exepectedPlayer = new Player()
        {
            PlayerNr = 1,
            InPlay = true,
            Home = A.Fake<Home>(),
            Pieces = []
        };
        
        GameOrchestrator orchestrator = new GameOrchestrator()
        {
            Players = [currentPlayer, exepectedPlayer],
            CurrentPlayer = 0,
            Board = A.Fake<Board>(),
            Die = A.Fake<DieBase>()
        };
        
        Piece piece = A.Fake<Piece>();
        
        // Act
        bool validMove = orchestrator.IsValidMove(piece);
        orchestrator.NextPlayer();
        
        // Assert
        validMove.Should().BeFalse();
        orchestrator.CurrentPlayer.Should().Be(exepectedPlayer.PlayerNr);
    }

    [Fact]
    public void SkipTurn_NoValidMoveButRolled6_ExtraTurn()
    {
        // Arrange
        Piece piece = A.Fake<Piece>();
        GameOrchestrator orchestrator = new GameOrchestrator()
        {
            Board = A.Fake<Board>(),
            Die = A.Fake<DieBase>(),
            Players = [],
            CurrentPlayer = 0
        };
        A.CallTo(() => orchestrator.Die.Roll()).Returns(6);
        
        // Act
        int rollResult = orchestrator.Die.Roll();
        bool validMove = orchestrator.IsValidMove(piece);
        orchestrator.NextPlayer();
        
        // Assert
        using AssertionScope scope = new();
        rollResult.Should().Be(6);
        validMove.Should().BeFalse();
        orchestrator.CurrentPlayer.Should().Be(0);
    }
}
   