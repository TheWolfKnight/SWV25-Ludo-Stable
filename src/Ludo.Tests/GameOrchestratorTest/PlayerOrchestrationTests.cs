using FakeItEasy;
using FluentAssertions;
using Ludo.Common.Models;
using Ludo.Common.Models.Dice;
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



        public static class TestHelpers
        {
            public static Player CreateDummyPlayer(byte playerNr)
            {
                return new Player
                {
                    PlayerNr = playerNr,
                    InPlay = true,
                    Pieces = new Piece[4],
                    Home = new Home
                    {
                        HomeTiles = new HomeTile[4],
                        Owner = new Player
                        {
                            PlayerNr = 99,
                            InPlay = true,
                            Pieces = new Piece[4],
                            Home = new Home
                            {
                                HomeTiles = new HomeTile[4],
                                Owner = default!,
                                Pieces = new System.Collections.Generic.List<Piece>()
                            }
                        },
                        Pieces = new System.Collections.Generic.List<Piece>()
                    }
                };
            }

            public static Board CreateDummyBoard()
            {
                return new Board
                {
                    Tiles = Array.Empty<TileBase>()
                };
            }


        }


        [Fact]
        public void DetermineStartingPlayer_ShouldReRollIfTie()
        {
            // Arrange
            var players = new[]
            {
                TestHelpers.CreateDummyPlayer(0),
                TestHelpers.CreateDummyPlayer(1),
                TestHelpers.CreateDummyPlayer(2),
                TestHelpers.CreateDummyPlayer(3),
            };

            var fakeDie = A.Fake<DieBase>();
            A.CallTo(() => fakeDie.Roll())
             .ReturnsNextFromSequence(3, 5, 2, 5, 6, 2);

            var orchestrator = new GameOrchestrator
            {
                Players = players,
                Die = fakeDie,
                Board = TestHelpers.CreateDummyBoard(),
                CurrentPlayer = 0
            };

            // Act
            orchestrator.DetermineStartingPlayer();

            // Assert
            orchestrator.CurrentPlayer.Should().Be(1);
        }

        [Fact]
        public void NextPlayer_WhenCurrentPlayerIs3_ShouldBe0()
        {
            // Arrange
            var players = new[]
            {
                TestHelpers.CreateDummyPlayer(0),
                TestHelpers.CreateDummyPlayer(1),
                TestHelpers.CreateDummyPlayer(2),
                TestHelpers.CreateDummyPlayer(3)
            };

            var orchestrator = new GameOrchestrator
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

        [Fact]
        public void NextPlayer_WhenCurrentPlayerIs1_ShouldBe2()
        {
            // Arrange
            var players = new[]
            {
                TestHelpers.CreateDummyPlayer(0),
                TestHelpers.CreateDummyPlayer(1),
                TestHelpers.CreateDummyPlayer(2),
                TestHelpers.CreateDummyPlayer(3)
            };

            var orchestrator = new GameOrchestrator
            {
                Players = players,
                Die = A.Fake<DieBase>(),
                Board = TestHelpers.CreateDummyBoard(),
                CurrentPlayer = 1
            };

            // Act
            orchestrator.NextPlayer();

            // Assert
            orchestrator.CurrentPlayer.Should().Be(2);
        }
    }

}