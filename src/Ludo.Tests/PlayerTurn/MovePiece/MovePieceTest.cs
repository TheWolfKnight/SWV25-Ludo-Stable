using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Ludo.Common.Models;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;

namespace Ludo.Tests.PlayerTurn.MovePiece
{
    public class MovePieceTest
    {
        [Fact]
        public void Move_rollsX_moveX()
        {
            //Arrange
            GameOrchestrator orchestrator = new GameOrchestrator
            {
                Board = new Board { Tiles = [A.Fake<TileBase>()]},
                Die = A.Fake<DieBase>(),
                CurrentPlayer = 0,
                Players = []
            };

            //Act
            int i = orchestrator.Die.Roll();
            bool moved = orchestrator.Board.Tiles[0].MovePiece(A.Fake<Piece>(), i);

            //Assert
            moved.Should().BeTrue();
        }

        [Fact]
        public void Move_NotRoll6AndPieceAtHome_CannotMoveOut()
        {
            //Arrange
            GameOrchestrator orchestrator = new GameOrchestrator
            {
                Board = new Board { Tiles = [A.Fake<TileBase>()] },
                Die = A.Fake<DieBase>(),
                CurrentPlayer = 0,
                Players = []
            };

            Home home = new Home
            {
                Owner = A.Fake<Player>(),
                Pieces = A.CollectionOfFake<Piece>(4).ToList(),
                HomeTiles = []
            };

            A.CallTo(() => orchestrator.Die.Roll()).Returns(3);


            //Act
            int i = orchestrator.Die.Roll();
            throw new NotImplementedException();

            //Assert
            home.Pieces.Should().HaveCount(4);
        }

        [Fact]
        public void Move_NotRoll6AndNotAPieceAtHome_cannotMoveout()
        {
            //Arrange
            GameOrchestrator orchestrator = new GameOrchestrator
            {
                Board = new Board { Tiles = [A.Fake<TileBase>()] },
                Die = A.Fake<DieBase>(),
                Players = [],
                CurrentPlayer = 0
            };

            Home home = new Home
            {
                Owner = A.Fake<Player>(),
                Pieces = [],
                HomeTiles = []
            };

            A.CallTo(() => orchestrator.Die.Roll()).Returns(3);

            //Act
            int i = orchestrator.Die.Roll();


            //Assert
            throw new NotImplementedException();
            home.Pieces.Should().BeEmpty();
        }

        [Fact]
        public void Move_Roll6andHasPieceAtHome_MoveOut()
        {
            //Arrange
            GameOrchestrator orchestrator = new GameOrchestrator
            {
                Board = new Board { Tiles = [A.Fake<TileBase>()] },
                Die = A.Fake<DieBase>(),
                CurrentPlayer = 0,
                Players = []
            };

            Home home = new Home
            {
                Owner = A.Fake<Player>(),
                Pieces = A.CollectionOfFake<Piece>(4).ToList(),
                HomeTiles = []
            };

            A.CallTo(() => orchestrator.Die.Roll()).Returns(6);

            //Act
            int i = orchestrator.Die.Roll();
            bool moved = orchestrator.Board.Tiles[0].MovePiece(home.Pieces[0], i); //Burde være void?

            //Assert
            home.Pieces.Should().HaveCount(3);

        }

        [Fact]
        public void Move_Roll6andNoneAtHome_DontMoveOut()
        {
            //Arrange
            GameOrchestrator orchestrator = new GameOrchestrator
            {
                Board = new Board { Tiles = [A.Fake<TileBase>()] },
                Die = A.Fake<DieBase>(),
                CurrentPlayer = 0,
                Players = []
            };

            Home home = new Home
            {
                Owner = A.Fake<Player>(),
                Pieces = [],
                HomeTiles = []
            };

            A.CallTo(() => orchestrator.Die.Roll()).Returns(6);

            //Act
            int i = orchestrator.Die.Roll();
            throw new NotImplementedException();

            //Assert
            home.Pieces.Should().BeEmpty();


        }
    }
}
