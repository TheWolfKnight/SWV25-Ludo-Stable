using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Enums;
using Ludo.Common.Models;
using Ludo.Common.Models.Dice;
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
      Piece piece = A.Fake<Piece>();
      DieBase die = A.Fake<DieBase>();
      StandardTile moveToTile = A.Fake<StandardTile>();

      StandardTile startTile = new()
      {
        Location = (0,0),
        PlayerNr = 0,
        Pieces = [piece],
        NextTile = moveToTile
      };

      //Act
      int rolled = die.Roll();
      startTile.MovePiece(piece, rolled);

      //Assert
      startTile.Pieces.Should().BeEmpty();
    }

    [Fact]
    public void Move_NotRoll6AndPieceAtHome_CannotMoveOut()
    {
      //Arrange
      Player player = A.Fake<Player>();
      DieBase die = A.Fake<DieBase>();

      HomeTile homeTile = new()
      {
        Location = (0, 0),
        Pieces = [],
        PlayerNr = 0,
        NextTile = A.Fake<StandardTile>()
      };

      Piece piece = new()
      {
        Owner = player,
        PieceState = PieceState.Home,
        CurrentTile = homeTile
      };

      homeTile.Pieces.Add(piece);

      Home home = new()
      {
        Owner = player,
        Pieces = [piece, A.Fake<Piece>()],
        HomeTiles = [homeTile]
      };

      A.CallTo(() => die.Roll()).Returns(3);
      int rolled = die.Roll();

      //Act
      home.HomeTiles[0].MovePiece(piece, rolled);

      //Assert
      using AssertionScope assertions = new();
      piece.PieceState.Should().Be(PieceState.Home);
      home.Pieces.Should().Contain(piece).And.HaveCount(2);
      homeTile.Pieces.Should().Contain(piece);
    }

    [Fact]
    public void Move_NotRoll6AndNotAPieceAtHome_CannotMoveOut()
    {
      //Arrange
      TileBase nextTile = A.Fake<StandardTile>();
      Player player = A.Fake<Player>();
      DieBase die = A.Fake<DieBase>();
      
      HomeTile homeTile = new()
      {
        Location = (0, 0),
        Pieces = [],
        PlayerNr = 0,
        NextTile = nextTile
      };
      
      Home home = new()
      {
        Owner = player,
        Pieces = [],
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
      home.Pieces.Should().BeEmpty();
      nextTile.Pieces.Should().BeEmpty();
    }

    [Fact]
    public void Move_Roll6andHasPieceAtHome_MoveOut()
    {
      //Arrange
      Player player = A.Fake<Player>();
      DieBase die = A.Fake<DieBase>();
      
      HomeTile homeTile = new()
      {
        Location = (0, 0),
        NextTile = A.Fake<StandardTile>(),
        Pieces = [],
        PlayerNr = 0
      };

      Piece piece = new()
      {
        Owner = player,
        CurrentTile = homeTile,
        PieceState = PieceState.Home
      };
      
      homeTile.Pieces.Add(piece);
      
      Home home = new()
      {
        Owner = player,
        Pieces = [piece],
        HomeTiles = [homeTile]
      };

      A.CallTo(() => die.Roll()).Returns(6);
      int i = die.Roll();

      //Act
      homeTile.MovePiece(piece, i);

      //Assert
      using AssertionScope scope = new();
      home.Pieces.Should().HaveCount(1);
      homeTile.Pieces.Should().BeEmpty();
      piece.PieceState.Should().Be(PieceState.OnBoard);
    }

    [Fact]
    public void Move_Roll6andNoneAtHome_DontMoveOut()
    {
      //Arrange
      TileBase nextTile = A.Fake<StandardTile>();
      Player player = A.Fake<Player>();
      DieBase die = A.Fake<DieBase>();
      
      HomeTile homeTile = new()
      {
        Location = (0, 0),
        Pieces = [],
        PlayerNr = 0,
        NextTile = nextTile
      };
      
      Home home = new()
      {
        Owner = player,
        Pieces = [],
        HomeTiles = [homeTile]
      };

      A.CallTo(() => die.Roll()).Returns(6);
      int i = die.Roll();

      //Act
      // Like the Unit test further up, this is an odd one
      // since we're checking if it failed when moving a piece
      // out, but no pieces exist
      homeTile.MovePiece(A.Fake<Piece>(), i);

      //Assert
      home.Pieces.Should().BeEmpty();
      nextTile.Pieces.Should().BeEmpty();
    }
  }
}
