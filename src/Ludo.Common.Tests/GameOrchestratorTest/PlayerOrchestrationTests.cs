using FakeItEasy;
using FluentAssertions;
using Ludo.Common.Models;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Tests.GameOrchestratorTest
{
  public class PlayerOrchestrationTests
  {
    public static IEnumerable<object[]> PlayerData =>
      new List<object[]>
      {
        new object[] { new byte[] { 0, 1, 2, 3 }, 0, 1 },
        new object[] { new byte[] { 0, 1, 2, 3 }, 1, 2 },
        new object[] { new byte[] { 0, 1, 2, 3 }, 2, 3 },
        new object[] { new byte[] { 0, 1, 2, 3 }, 3, 0 }
      };

    public static IEnumerable<object[]> PlayerCycleData => new List<object[]>
    {
      new object[] { TestHelpers.CreateDummyPlayer(0) },
      new object[] { TestHelpers.CreateDummyPlayer(1) },
      new object[] { TestHelpers.CreateDummyPlayer(2) },
      new object[] { TestHelpers.CreateDummyPlayer(3) }
    };

    [Theory]
    [MemberData(nameof(PlayerData))]
    public void NextPlayer_ShouldCycleThroughPlayers(byte[] playerNumbers, byte currentPlayer, byte expectedNextPlayer)
    {
      // Arrange
      var players = new Player[playerNumbers.Length];
      for (int i = 0; i < playerNumbers.Length; i++)
      {
        players[i] = new Player
        {
          PlayerNr = playerNumbers[i],
          InPlay = true,
          Pieces = new Piece[4], // Assuming each player has 4 pieces
          Home = new Home
          {
            HomeTiles = new HomeTile[4], // Assuming each home has 4 tiles
            Owner = players[i], // Set Owner in the object initializer
          }
        };
      }

      GameOrchestrator gameOrchestrator = new()
      {
        CurrentPlayer = currentPlayer,
        Players = players,
        Die = A.Fake<DieBase>(),
        Board = A.Fake<Board>()
      };

      // Act
      gameOrchestrator.NextPlayer();

      // Assert
      gameOrchestrator.CurrentPlayer.Should().Be(expectedNextPlayer);
    }

    [Fact]
    public void Starting_RollForStartAndTwoRollSame_ShouldRequireReRoll()
    {
      int[] playerRolls = [3, 5, 2, 5];

      // Act
      byte[] returnedRollers = GameOrchestrator.DetermineStartingPlayer(playerRolls);

      // Assert
      returnedRollers.Should().HaveCount(2);
    }

    [Theory]
    [MemberData(nameof(PlayerCycleData))]
    public void NextPlayer_WhenCurrentPlayerIs3_ShouldBe0(Player[] players)
    {
      // Arrange
      GameOrchestrator orchestrator = new()
      {
        Players = players,
        Die = A.Fake<DieBase>(),
        Board = TestHelpers.CreateDummyBoard(),
        CurrentPlayer = 3
      };

      // Act
      orchestrator.NextPlayer();

      // Assert
      orchestrator.CurrentPlayer.Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(PlayerCycleData))]
    public void NextPlayer_WhenCurrentPlayerIs1_ShouldBe2(Player[] players)
    {
      // Arrange
      GameOrchestrator orchestrator = new()
      {
        Players = players,
        Die = A.Fake<DieD6>(),
        Board = A.Fake<Board>(),
        CurrentPlayer = 1
      };

      // Act
      orchestrator.NextPlayer();

      // Assert
      orchestrator.CurrentPlayer.Should().Be(2);
    }

    #region Helpers

    private static class TestHelpers
    {
      public static Player CreateDummyPlayer(byte playerNr)
      {
        return new Player
        {
          PlayerNr = playerNr,
          InPlay = true,
          Pieces = A.CollectionOfFake<Piece>(4).ToArray(),
          Home = A.Fake<Home>()
        };
      }

      public static Board CreateDummyBoard()
      {
        return new Board
        {
          X = 0,
          Y = 0,
          Tiles = []
        };
      }
    }

    #endregion
  }
}
