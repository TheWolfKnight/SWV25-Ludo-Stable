using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Models.Player;
using Ludo.Common.Enums;

namespace Ludo.Tests.TileTests;

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
    using AssertionScope scope = new();
    piece.CurrentTile.Should().Be(GetTileAt(tile, 3));
    GetTileAt(tile, 3).Pieces.Should().Contain(piece);
    tile.Pieces.Should().BeEmpty();
  }

  [Fact]
  public void DriveWayTile_Moves4Tiles_DriveWayContains3Tiles_PieceMovesBackwards()
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
    tile.MovePiece(piece, 4);

    //Assert
    using AssertionScope scope = new();
    piece.CurrentTile.Should().Be(tile);
    tile.Pieces.Should().Contain(piece);
  }

#region Helpers
  private DriveWayTile GenerateDriveWay(int depth, byte playerAlligiance)
  {
    DriveWayTile tail = new DriveWayTile
    {
      PlayerNr = playerAlligiance,
      Location = (1, 1),
      Pieces = [],
      NextTile = null!,
      PreviusTile = null!,
    };

    DriveWayTile currentTile = tail;

    for (int i = 0; i < depth; ++i)
    {
      DriveWayTile head = new DriveWayTile
      {
        PlayerNr = playerAlligiance,
        Location = (1, 1),
        Pieces = [],
        NextTile = null!,
        PreviusTile = null!,
      };

      tail.PreviusTile = head;
      head.NextTile = tail;

      currentTile = head;
    }

    return currentTile;
  }

  private DriveWayTile GetTileAt(DriveWayTile tile, int depth)
  {
    DriveWayTile currentTile = tile;
    for (int i = 0; i < depth; i++)
      currentTile = (DriveWayTile)tile.NextTile;

    return currentTile;
  }
#endregion // Helpers
}

