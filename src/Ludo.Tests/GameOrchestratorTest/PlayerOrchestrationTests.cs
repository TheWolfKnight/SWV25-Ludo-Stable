using FakeItEasy;
using FluentAssertions;
using Ludo.Common.Models;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;

namespace Ludo.Tests.GameOrchestratorTest
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
            Pieces = new List<Piece>()
          }
        };
      }

      var gameOrchestrator = new GameOrchestrator
      {
        CurrentPlayer = currentPlayer,
        Players = players,
        Die = A.Fake<DieBase>(),
        Board = A.Fake<Board>()
      };

      // Act
      gameOrchestrator.NextPlayer();

      // Assert
      expectedNextPlayer.Should().Be(gameOrchestrator.CurrentPlayer);
    }



        [Fact]
        public void DetermineStartingPlayer_ShouldReRollIfTie()
        {
            // Arrange
            //Create 4 players with only PlayerNr.

            var players = new[]
            {
                new Player { PlayerNr = 0 },
                new Player { PlayerNr = 1 },
                new Player { PlayerNr = 2 },
                new Player { PlayerNr = 3 }
            };

            //Fake a die with a specific hitting sequence

            // First 4 rolls: 3, 5, 2, 5 => silence between players 1 and 3 (both roll 5)
            // Next 2 rolls: 6, 2 => player 1 wins tie-break (6 > 2).

            var fakeDie = A.Fake<DieBase>();
            A.CallTo(() => fakeDie.Roll())
                .ReturnsNextFromSequence(3, 5, 2, 5, 6, 2);

            // Create your GameOrchestrator and associate your players + fake dice

            var gameOrchestrator = new GameOrchestrator
            {
                Players = players,
                Die = fakeDie
            };

            // Act
            // Call the method that determines the starting player (including tie-break logic).
            gameOrchestrator.DetermineStartingPlayer();

            // Assert
            // Since Player 1 wins the tie-break, CurrentPlayer should be 1.
            gameOrchestrator.CurrentPlayer.Should().Be(1);
        }

        public class PlayerOrchestrationTests
        {
            [Fact]
            public void NextPlayer_WhenCurrentPlayerIs1_ShouldBe2()
            {
                // Arrange
                var players = new[]
                {
                new Player { PlayerNr = 0 },
                new Player { PlayerNr = 1 },
                new Player { PlayerNr = 2 },
                new Player { PlayerNr = 3 }
            };

                var gameOrchestrator = new GameOrchestrator
                {
                    Players = players,
                    CurrentPlayer = 1
                };

                // Act
                gameOrchestrator.NextPlayer();

                // Assert
                gameOrchestrator.CurrentPlayer.Should().Be(2);
            }

            [Fact]
            public void NextPlayer_WhenCurrentPlayerIs3_ShouldBe0()
            {
                // Arrange
                var players = new[]
                {
                new Player { PlayerNr = 0 },
                new Player { PlayerNr = 1 },
                new Player { PlayerNr = 2 },
                new Player { PlayerNr = 3 }
            };

                var gameOrchestrator = new GameOrchestrator
                {
                    Players = players,
                    CurrentPlayer = 3
                };

                // Act
                gameOrchestrator.NextPlayer();

                // Assert
                gameOrchestrator.CurrentPlayer.Should().Be(0);
            }
        }
    }
}