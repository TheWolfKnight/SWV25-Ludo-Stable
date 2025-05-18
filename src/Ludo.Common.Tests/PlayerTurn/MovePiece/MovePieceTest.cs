using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Enums;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Tests.PlayerTurn.MovePiece
{
  public class MovePieceTest
  {
    [Fact]
    public void Move_rollsX_moveX()
    {
      //Arrange
      Player player = new Player
      {
        PlayerNr = 1,
        InPlay = true,
        Home = null!,
        Pieces = new Piece[4]
      };
      Piece piece = new Piece
      {
        CurrentTile = null!,
        Owner = player,
        PieceState = PieceState.OnBoard
      };
      player.Pieces[0] = piece;

      StandardTile startTile = new()
      {
        IndexInBoard = 1,
        Pieces = [piece],
        NextTile = GenerateFakeTiles(7)
      };

      DateTime dt = DateTime.Now;
      Random rnd = new Random((int)(dt.Ticks/dt.Day));
      int rolled = rnd.Next(0, 6) + 1;
      //Act
      startTile.MovePiece(piece, rolled);

      //Assert
      using AssertionScope scope = new();
      MovementTile expectedTile = GetTileAt(startTile, rolled)!;

      startTile.Pieces.Should().BeEmpty();
      expectedTile.Pieces.Should().Contain(piece);
    }

    [Fact]
    public void Move_NotRoll6AndNotAPieceAtHome_CannotMoveOut()
    {
      //Arrange
      Player player = A.Fake<Player>();
      DieBase die = A.Fake<DieBase>();
      
      HomeTile homeTile = new()
      {
        IndexInBoard = 1,
        Pieces = [],
        PlayerNr = 0,
        NextTile = new StandardTile
        {
          IndexInBoard = 2,
          NextTile = null!,
          Pieces = []
        }
      };
      
      Home home = new()
      {
        Owner = player,
        HomeTiles = [homeTile]
      };

      A.CallTo(() => die.Roll()).Returns(3);
      int i = die.Roll();

      //Act
      // This one is a bit odd, since I'm required to use a 
      // piece when moving out (since ofc), but for testing
      // I'm using a Fake Piece
      homeTile.MovePiece(A.Fake<Piece>(), i);

      //Assert
      homeTile.Pieces.Should().BeEmpty();
      homeTile.NextTile.Pieces.Should().BeEmpty();
    }

    [Fact]
    public void Move_Roll6andHasPieceAtHome_MoveOut()
    {
      //Arrange
      DieBase die = A.Fake<DieBase>();
      Player player = new Player
      {
        Home = new()
        {
          Owner = null!,
          HomeTiles = new HomeTile[4]
        },
        InPlay = true,
        Pieces = new Piece[4],
        PlayerNr = 1
      };
      Piece piece = new()
      {
        Owner = player,
        CurrentTile = null!,
        PieceState = PieceState.Home
      };
      player.Pieces[0] = piece;

      HomeTile homeTile = new()
      {
        IndexInBoard = 1,
        NextTile = new StandardTile
        {
          IndexInBoard = 2,
          NextTile = null!,
          Pieces = []
        },
        Pieces = [piece],
        PlayerNr = 0
      };
      piece.CurrentTile = homeTile;
      player.Home.HomeTiles[0] = homeTile;

      A.CallTo(() => die.Roll()).Returns(6);
      int i = die.Roll();

      //Act
      homeTile.MovePiece(piece, i);

      //Assert
      using AssertionScope scope = new();
      homeTile.Pieces.Should().BeEmpty();
      piece.PieceState.Should().Be(PieceState.OnBoard);
    }

    #region Helpers
    private MovementTile GenerateFakeTiles(int depth = 1)
    {
      StandardTile tail = new StandardTile
      {
        IndexInBoard = 1,
        Pieces = [],
        NextTile = null!,
      };
      StandardTile current = tail;

      for (int i = 1; i < depth; ++i)
      {
        StandardTile head = new StandardTile
        {
          IndexInBoard = 1,
          Pieces = [],
          NextTile = current
        };

        current = head;
      }

      return current;
    }

    private StandardTile? GetTileAt(StandardTile tile, int depth)
    {
      StandardTile currentTile = tile;

      for (int i = 0; i < depth; i++)
        currentTile = (StandardTile)currentTile.NextTile;

      return currentTile;
    }
    #endregion
  }
}
