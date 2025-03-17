using FakeItEasy;
using FluentAssertions;
using Ludo.Common.Models;
using Ludo.Common.Models.Player;

namespace Ludo.Tests.PlayerTurn.SkipTurn;

public class SkipTurnTests
{
    [Fact]
    public void SkipTurn_NoValidMove_SkipTurn()
    {
        // Arrange
        GameOrchestrator orchestrator = A.Fake<GameOrchestrator>();
        Piece piece = A.Fake<Piece>();
        
        // Act
        bool validMove = orchestrator.IsValidMove(piece);
        
        // Assert
        validMove.Should().BeFalse();
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
        
        // Assert
        rollResult.Should().Be(6);
        validMove.Should().BeFalse();
    }
}
   