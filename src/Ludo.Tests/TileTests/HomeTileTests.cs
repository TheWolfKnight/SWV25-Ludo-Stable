using FluentAssertions;
using FluentAssertions.Execution;
using Ludo.Common.Enums;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;

namespace Ludo.Tests.TileTests;

public class HomeTileTests
{
  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  [InlineData(4)]
  [InlineData(5)]
  public void HomeTile_PieceDoesNotMoveOut_When6NotRolled(int roll)
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
      PieceState = PieceState.Home
    };

    HomeTile tile = new HomeTile
    {
      PlayerNr = 1,
      Location = (1, 1),
      Pieces = [piece],
      NextTile = new StandardTile
      {
        Location = (1, 1),
        Pieces = [],
        NextTile = null!,
      }
    };

    //Act
    tile.MovePiece(piece, roll);

    //Assert
    using AssertionScope scope = new();
    piece.PieceState.Should().Be(PieceState.Home);
    piece.CurrentTile.Should().Be(tile);
    tile.Pieces.Should().Contain(piece);
    tile.NextTile.Pieces.Should().BeEmpty();
  }

  [Fact]
  public void HomeTile_PieceShouldMoveOut_When6Rolled()
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
      PieceState = PieceState.Home
    };

    HomeTile tile = new HomeTile
    {
      PlayerNr = 1,
      Location = (1, 1),
      Pieces = [piece],
      NextTile = new StandardTile
      {
        Location = (1, 1),
        Pieces = [],
        NextTile = null!,
      }
    };

    //Act
    tile.MovePiece(piece, 6);

    //Assert
    using AssertionScope scope = new();
    piece.PieceState.Should().Be(PieceState.OnBoard);
    piece.CurrentTile.Should().Be(tile.NextTile);
    tile.Pieces.Should().BeEmpty();
    tile.NextTile.Pieces.Should().Contain(piece);
  }
}

