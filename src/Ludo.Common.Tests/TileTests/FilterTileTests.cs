using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Models.Player;
using Ludo.Common.Enums;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Tests.TileTests;

public class FilterTileTests
{
  [Fact]
  public void FilterTile_WrongPlayerNrMovesOverFilter_MovesToNextStandard()
  {
    //Arrange
    Player player = new Player
    {
      PlayerNr = 2,
      InPlay = true,
      Pieces = new Piece[1],
      Home = null!,
    };

    Piece piece = new Piece
    {
      Owner = player,
      CurrentTile = null!,
      PieceState = PieceState.OnBoard
    };
    player.Pieces[0] = piece;

    FilterTile tile = SetupFilterTile(piece, 1);
    piece.CurrentTile = tile;

    //Act
    tile.MovePiece(piece, 1);

    //Assert
    using AssertionScope scope = new();
    piece.CurrentTile.Should().Be(tile.NextTile);
    tile.Pieces.Should().BeEmpty();
    tile.NextTile.Pieces.Should().Contain(piece);
    tile.FilterdTile.Pieces.Should().BeEmpty();
  }

  [Fact]
  public void FilterTile_AllingedPlayerNrMovesOverFilter_MovesToFilterdTile()
  {
    Player player = new Player
    {
      PlayerNr = 1,
      InPlay = true,
      Pieces = new Piece[1],
      Home = null!,
    };

    Piece piece = new Piece
    {
      Owner = player,
      CurrentTile = null!,
      PieceState = PieceState.OnBoard
    };
    player.Pieces[0] = piece;

    FilterTile tile = SetupFilterTile(piece, 1);
    piece.CurrentTile = tile;

    //Act
    tile.MovePiece(piece, 1);

    //Assert
    using AssertionScope scope = new();
    piece.CurrentTile.Should().Be(tile.FilterdTile);
    tile.Pieces.Should().BeEmpty();
    tile.NextTile.Pieces.Should().BeEmpty();
    tile.FilterdTile.Pieces.Should().Contain(piece);
  }

#region Helpers
  private FilterTile SetupFilterTile(Piece piece, byte playerAllegiance)
  {
    FilterTile result = new FilterTile
    {
      IndexInBoard = 1,
      PlayerNr = playerAllegiance,
      Pieces = [piece],
      NextTile = new StandardTile
      {
        IndexInBoard = 1,
        Pieces = [],
        NextTile = null!
      },
      FilterdTile = new DriveWayTile
      {
        PlayerNr = playerAllegiance,
        IndexInBoard = 1,
        Pieces = [],
        NextTile = null!,
        PreviousTile = null,
      }
    };

    return result;
  }
#endregion // HELPERS
}

