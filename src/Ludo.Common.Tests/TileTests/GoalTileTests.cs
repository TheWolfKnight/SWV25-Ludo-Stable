using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Models.Player;
using Ludo.Common.Enums;


namespace Ludo.Common.Tests.TileTests;

public class GoalTileTests
{
  [Fact]
  public void GoalTile_PieceLandsInGoalTile_GetSetToInGoal()
  {
    //Arrange
    Piece piece = new Piece
    {
      Owner = new Player
      {
        PlayerNr = 1,
        InPlay = true,
        Pieces = [],
        Home = null!,
      },
      CurrentTile = null!,
      PieceState = PieceState.OnBoard,
    };

    DriveWayTile tile = new DriveWayTile
    {
      PlayerNr = 1,
      IndexInBoard = 1,
      Pieces = [piece],
      NextTile = null!,
      PreviousTile = null!,
    };
    piece.CurrentTile = tile;

    GoalTile goal = new GoalTile
    {
      PlayerNr = 1,
      IndexInBoard = 1,
      Pieces = [],
      PreviousTile = tile,
    };
    tile.NextTile = goal;

    //Act
    tile.MovePiece(piece, 1);

    //Assert
    using AssertionScope scope = new();
    tile.Pieces.Should().BeEmpty();
    piece.CurrentTile.Should().Be(goal);
    piece.PieceState.Should().Be(PieceState.InGoal);
    goal.Pieces.Should().Contain(piece);
  }

  [Fact]
  public void GoalTile_PieceRolls2InFrontOfGoal_MovesBack1TileFromGoal()
  {
    //Arrange
    Piece piece = new Piece
    {
      Owner = new Player
      {
        PlayerNr = 1,
        InPlay = true,
        Pieces = [],
        Home = null!,
      },
      CurrentTile = null!,
      PieceState = PieceState.OnBoard,
    };

    DriveWayTile tile = new DriveWayTile
    {
      PlayerNr = 1,
      IndexInBoard = 1,
      Pieces = [piece],
      NextTile = null!,
      PreviousTile = null!,
    };
    piece.CurrentTile = tile;

    GoalTile goal = new GoalTile
    {
      PlayerNr = 1,
      IndexInBoard = 1,
      Pieces = [],
      PreviousTile = tile,
    };
    tile.NextTile = goal;

    //Act
    tile.MovePiece(piece, 2);

    //Assert
    using AssertionScope scope = new();
    tile.Pieces.Should().Contain(piece);
    piece.CurrentTile.Should().Be(tile);
    piece.PieceState.Should().Be(PieceState.OnBoard);
    goal.Pieces.Should().BeEmpty();
  }
}
