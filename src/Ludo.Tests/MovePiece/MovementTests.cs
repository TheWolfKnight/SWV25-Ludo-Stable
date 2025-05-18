using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Models.Player;
using Ludo.Common.Enums;
using Ludo.Common.Models.Dice;

namespace Ludo.Tests.MovePiece;

public class MovementTests
{
  [Theory]
  [InlineData(3, 5, 5)]
  [InlineData(2, 1, 3)]
  public void Movement_GivenRollX_DoNotMoveY(int roll, int check, int tileDepth)
  {
    // Arrange
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
    DieBase die = A.Fake<DieBase>();

    StandardTile startTile = new StandardTile
    {
      IndexInBoard = 1,
      Pieces = [piece],
      NextTile = GenerateFakeTiles(tileDepth)
    };

    piece.CurrentTile = startTile;
    A.CallTo(() => die.Roll()).Returns(roll);

    // Act
    startTile.MovePiece(piece, die.Roll());

    // Assert
    using AssertionScope scope = new();
    GetTileAt(startTile, check)?.Pieces.Should().BeEmpty();
    piece.CurrentTile.Should().NotBe(GetTileAt(startTile, check));
  }

  [Theory]
  [InlineData(3, 5)]
  public void Movement_GivenRoll3_Move3(int roll, int tileDepth)
  {
    // Arrange
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
    DieBase die = A.Fake<DieBase>();

    StandardTile startTile = new StandardTile
    {
      IndexInBoard = 1,
      Pieces = [piece],
      NextTile = GenerateFakeTiles(tileDepth)
    };

    piece.CurrentTile = startTile;
    A.CallTo(() => die.Roll()).Returns(roll);

    // Act
    startTile.MovePiece(piece, die.Roll());

    // Assert
    using AssertionScope scepe = new();
    MovementTile expectedTile = GetTileAt(startTile, roll)!;

    expectedTile.Pieces.Should().HaveCount(1);
    piece.CurrentTile.Should().Be(expectedTile);
  }

  [Fact]
  public void Movement_GivenBlueLandsOn2Red_SendBlueHome()
  {
    // Arrange
    Home blueHome = A.Fake<Home>();
    HomeTile blueHomeTile = new HomeTile
    {
      IndexInBoard = 0,
      NextTile = null!,
      Pieces = [],
      PlayerNr = 2,
    };

    A.CallTo(() => blueHome.GetFirstAvailableHomeTile()).Returns(blueHomeTile);

    Player redPlayer = new()
    {
      PlayerNr = 1,
      InPlay = true,
      Pieces = [],
      Home = null!
    };
    Piece red = new Piece
    {
      Owner = redPlayer,
      CurrentTile = null!,
      PieceState = PieceState.OnBoard,
    };
    Player bluePlayer = new()
    {
      PlayerNr = 2,
      InPlay = true,
      Pieces = [],
      Home = blueHome
    };
    Piece blue = new Piece
    {
      Owner = bluePlayer,
      CurrentTile = null!,
      PieceState = PieceState.OnBoard
    };

    StandardTile blueStart = new()
    {
      IndexInBoard = 1,
      Pieces = [blue],
      NextTile = new StandardTile
      {
        IndexInBoard = 1,
        Pieces = [red, red],
        NextTile = null!
      }
    };

    red.CurrentTile = blueStart.NextTile;
    blue.CurrentTile = blueStart;
    // Act
    blueStart.MovePiece(blue, 1);

    // Assert
    using AssertionScope scope = new();
    blue.PieceState.Should().Be(PieceState.Home);
    red.PieceState.Should().Be(PieceState.OnBoard);
  }

  [Fact]
  public void Movement_GivenRedLandsOnBlue_SendBlueHome()
  {
    // Arrange
    Home blueHome = A.Fake<Home>();
    HomeTile blueHomeTile = new HomeTile
    {
      IndexInBoard = 0,
      NextTile = null!,
      Pieces = [],
      PlayerNr = 2,
    };

    A.CallTo(() => blueHome.GetFirstAvailableHomeTile()).Returns(blueHomeTile);

    Player redPlayer = new()
    {
      PlayerNr = 1,
      InPlay = true,
      Pieces = [],
      Home = null!
    };
    Piece red = new Piece
    {
      Owner = redPlayer,
      CurrentTile = null!,
      PieceState = PieceState.OnBoard,
    };
    Player bluePlayer = new()
    {
      PlayerNr = 2,
      InPlay = true,
      Pieces = [],
      Home = blueHome
    };
    Piece blue = new Piece
    {
      Owner = bluePlayer,
      CurrentTile = null!,
      PieceState = PieceState.OnBoard
    };

    StandardTile redStart = new()
    {
      IndexInBoard = 1,
      Pieces = [red],
      NextTile = new StandardTile
      {
        IndexInBoard = 1,
        Pieces = [blue],
        NextTile = null!
      }
    };

    red.CurrentTile = redStart;
    blue.CurrentTile = redStart.NextTile;

    // Act
    redStart.MovePiece(red, 1);

    // Assert
    using AssertionScope scope = new();
    red.PieceState.Should().Be(PieceState.OnBoard);
    blue.PieceState.Should().Be(PieceState.Home);
  }

  [Fact]
  public void Movement_GivenPieceIs1FromGoal_AndDieRolls1_SendPieceIntoGoal()
  {
    // Arrange
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

    DriveWayTile tile = new DriveWayTile
    {
      PlayerNr = 1,
      IndexInBoard = 1,
      NextTile = null!,
      PreviousTile = null!,
      Pieces = [piece]
    };
    GoalTile goal = new()
    {
      PlayerNr = 1,
      IndexInBoard = 1,
      PreviousTile = tile,
      Pieces = []
    };

    tile.NextTile = goal;
    piece.CurrentTile = tile;

    // Act
    tile.MovePiece(piece, 1);

    // Assert
    using AssertionScope scope = new();
    piece.PieceState.Should().Be(PieceState.InGoal);
    goal.Pieces.Should().HaveCount(1).And.Contain(piece);
  }

  [Fact]
  public void Movement_GivenPieceIs1FromGoal_AndDieRolls2_SendPiece1FromGoal()
  {
    // Arrange
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

    DriveWayTile tile = new DriveWayTile
    {
      PlayerNr = 1,
      IndexInBoard = 1,
      NextTile = null!,
      PreviousTile = null!,
      Pieces = [piece]
    };

    GoalTile goal = new()
    {
      PlayerNr = 1,
      IndexInBoard = 1,
      Pieces = [],
      PreviousTile = tile
    };

    tile.NextTile = goal;
    piece.CurrentTile = tile;

    // Act
    tile.MovePiece(piece, 2);

    // Assert
    using AssertionScope scope = new();
    piece.PieceState.Should().Be(PieceState.OnBoard);
    goal.Pieces.Should().BeEmpty();
    tile.Pieces.Should().HaveCount(1).And.Contain(piece);
  }

  [Fact]
  public void Movement_GivenARollOf3_WithNoLegalMoves_DoNotExecuteMove()
  {
    // Arrange
    Player player = A.Fake<Player>();
    Piece piece1 = new()
    {
      CurrentTile = null!,
      Owner = player,
      PieceState = PieceState.OnBoard
    };
    Piece piece2 = new()
    {
      CurrentTile = null!,
      Owner = player,
      PieceState = PieceState.OnBoard
    };

    StandardTile tile = new StandardTile
    {
      IndexInBoard = 1,
      Pieces = [piece1],
      NextTile = new StandardTile
      {
        IndexInBoard = 1,
        NextTile = GenerateFakeTiles(2),
        Pieces = [piece2]
      }
    };

    piece1.CurrentTile = tile;
    piece2.CurrentTile = tile.NextTile;

    // Act
    tile.MovePiece(piece1, 3);

    // Assert
    piece1.CurrentTile.Should().Be(tile);
    tile.NextTile.Pieces.Should().NotContain(piece1);
  }

  [Fact]
  public void Movement_GivenARollOf3_WithLegalMoves_ExecuteMovement()
  {
    // Arrange
    Piece piece1 = new()
    {
      CurrentTile = null!,
      Owner = new Player
      {
        PlayerNr = 1,
        Home = null!,
        InPlay = true,
        Pieces = new Piece[4]
      },
      PieceState = PieceState.OnBoard
    };
    piece1.Owner.Pieces[0] = piece1;

    StandardTile tile = new StandardTile
    {
      IndexInBoard = 1,
      Pieces = [piece1],
      NextTile = new StandardTile
      {
        IndexInBoard = 1,
        NextTile = GenerateFakeTiles(2),
        Pieces = []
      }
    };
    piece1.CurrentTile = tile;

    // Act
    tile.MovePiece(piece1, 3);

    // Assert
    using AssertionScope scope = new();
    MovementTile expectedTile = GetTileAt(tile, 3)!;

    piece1.CurrentTile.Should().Be(expectedTile);
    expectedTile.Pieces.Should().Contain(piece1);
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

