//using System.Collections;
//using FakeItEasy;
//using FluentAssertions;
//using FluentAssertions.Execution;
//using Ludo.Common.Models;
//using Ludo.Common.Models.Dice;
//using Ludo.Common.Models.Player;

//namespace Ludo.Tests.PlayerTurn.SkipTurn;

//public class SkipTurnTests
//{
//  [Theory]
//  [ClassData(typeof(SkipTurnTestData))]
//  public void SkipTurn_NoValidMove_ReturnFalse(GameOrchestrator orchestrator, Player expectedPlayer)
//  {
//    // Arrange
//    Piece piece = A.Fake<Piece>();

//    // Act
//    //bool validMove = orchestrator.HasValidMove(piece);

//    // Assert
//    orchestrator.CurrentPlayer.Should().Be(expectedPlayer.PlayerNr);
//  }

//  [Fact]
//  public void SkipTurn_NoValidMoveButRolled6_ExtraTurn()
//  {
//      // Arrange
//      Piece piece = A.Fake<Piece>();
//      GameOrchestrator orchestrator = new GameOrchestrator()
//      {
//          Board = A.Fake<Board>(),
//          Die = A.Fake<DieBase>(),
//          Players = [],
//          CurrentPlayer = 0
//      };
//      A.CallTo(() => orchestrator.Die.Roll()).Returns(6);
//      //A.CallTo(() => orchestrator.HasValidMove(piece)).Returns(false); 
        
//      // Act
//      int die = orchestrator.Die.Roll();
//      //bool validMove = orchestrator.HasValidMove(piece);
//      orchestrator.NextPlayer();
        
//      // Assert
//      orchestrator.CurrentPlayer.Should().Be(0);
//  }
//}

//public class SkipTurnTestData : IEnumerable<object[]>
//{
//  public IEnumerator<object[]> GetEnumerator()
//  {
//    Player currentPlayer = new Player()
//    {
//      PlayerNr = 0,
//      InPlay = true,
//      Home = A.Fake<Home>(),
//      Pieces = []
//    };

//    Player expectedPlayer = new Player()
//    {
//      PlayerNr = 1,
//      InPlay = true,
//      Home = A.Fake<Home>(),
//      Pieces = []
//    };

//    yield return new object[]
//    {
//      new GameOrchestrator()
//      {
//        Players = [currentPlayer, expectedPlayer],
//        CurrentPlayer = 0,
//        Board = A.Fake<Board>(),
//        Die = A.Fake<DieBase>()
//      },
//      expectedPlayer
//    };
//  }

//  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  
//}
