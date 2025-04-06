using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Models.Player;
using Ludo.Common.Enums;


namespace Ludo.Tests.TileTests;

public class GoalTileTests
{
  [Fact]
  public void GoalTile_PieceLandsInGoalTile_GetSetToInGoal()
  {
    //Arrange
    Piece piece = new Piece
    {
      Owner = null!,
      CurrentTile = null!,
      PieceState = PieceState.OnBoard,
    };

    DriveWayTile tile = new DriveWayTile
    {
      Location = (1, 1),
      Pieces = [piece],
      NextTile = null!,
      PreviusTile = null!,
    };
    piece.CurrentTile = tile;

    GoalTile goal = new GoalTile
    {
      Location = (1, 1),
      Pieces = [],
      PreviusTile = tile,
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
  public void GoalTile_PieceRolls2InFrontOfGoal_MovesBack1SquareFromGoal()
  {
    //Arrange
    Piece piece = new Piece
    {
      Owner = null!,
      CurrentTile = null!,
      PieceState = PieceState.OnBoard,
    };

    DriveWayTile tile = new DriveWayTile
    {
      Location = (1, 1),
      Pieces = [piece],
      NextTile = null!,
      PreviusTile = null!,
    };
    piece.CurrentTile = tile;

    GoalTile goal = new GoalTile
    {
      Location = (1, 1),
      Pieces = [],
      PreviusTile = tile,
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
