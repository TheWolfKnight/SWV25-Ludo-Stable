using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Models.Player;
using Ludo.Common.Enums;

namespace Ludo.Common.Tests.TileTests;

public class DriveWayTileTests
{
  [Fact]
  public void DriveWayTile_Moves2Tiles_DriveWayContains3Tiles_PieceDoesNotMoveBackwards()
  {
    //Arrange
    Piece piece = new Piece
    {
      Owner = new Player
      {
        PlayerNr = 1,
        InPlay = true,
        Pieces = new Piece[1],
        Home = null!,
      },
      CurrentTile = null!,
      PieceState = PieceState.OnBoard,
    };

    DriveWayTile tile = GenerateDriveWay(3, 1);
    tile.Pieces.Add(piece);
    piece.CurrentTile = tile;

    //Act
    tile.MovePiece(piece, 2);

    //Assert
    DriveWayTile target = GetTileAt(tile, 3);
    using AssertionScope scope = new();
    piece.CurrentTile.Should().Be(target);
    target.Pieces.Should().Contain(piece);
    tile.Pieces.Should().BeEmpty();
  }

  [Fact]
  public void DriveWayTile_Moves5Tiles_DriveWayContains3Tiles_PieceMovesBackwards()
  {
    //Arrange
    Piece piece = new Piece
    {
      Owner = new Player
      {
        PlayerNr = 1,
        InPlay = true,
        Pieces = new Piece[1],
        Home = null!,
      },
      CurrentTile = null!,
      PieceState = PieceState.OnBoard,
    };

    DriveWayTile tile = GenerateDriveWay(2, 1);
    tile.NextTile = new GoalTile
    {
      IndexInBoard = 1,
      Pieces = [],
      PlayerNr = 1,
      PreviousTile = tile
    };
    tile.Pieces.Add(piece);
    piece.CurrentTile = tile;

    //Act
    tile.MovePiece(piece, 4);

    //Assert
    using AssertionScope scope = new();
    piece.CurrentTile.Should().Be(tile);
    tile.Pieces.Should().Contain(piece);
  }

  #region Helpers
  private DriveWayTile GenerateDriveWay(int depth, byte playerAlligiance)
  {
    DriveWayTile head = new DriveWayTile
    {
      PlayerNr = playerAlligiance,
      IndexInBoard = 1,
      Pieces = [],
      NextTile = null!,
      PreviousTile = null!,
    };

    DriveWayTile currentTile = head;

    for (int i = 0; i < depth - 1; ++i)
    {
      DriveWayTile tail = new DriveWayTile
      {
        PlayerNr = playerAlligiance,
        IndexInBoard = 1,
        Pieces = [],
        NextTile = null!,
        PreviousTile = null!,
      };

      currentTile.NextTile = tail;
      tail.PreviousTile = currentTile;

      currentTile = tail;
    }

    return head;
  }

  private DriveWayTile GetTileAt(DriveWayTile tile, int depth)
  {
    DriveWayTile currentTile = tile;
    for (int i = 0; i < depth - 1; i++)
      currentTile = (DriveWayTile)currentTile.NextTile;

    return currentTile;
  }
  #endregion // Helpers
}

