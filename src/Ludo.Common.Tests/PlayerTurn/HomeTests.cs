using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Enums;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Ludo.Common.Tests;

public class HomeTests
{
  [Theory]
  [MemberData(nameof(GetSetupAvailableSpace), (byte)1, 10)]
  public void Home_TryGetEmptyHome_WithHomeTileAvailable_ReturnFirstHomeTile(Home home, HomeTile expectedTile)
  {
    //Act
    HomeTile tile = home.GetFirstAvailableHomeTile();

    using AssertionScope scope = new();
    tile.Should().Be(expectedTile);
  }

  [Fact]
  public void Home_TryGetEmptyHome_WithoutHomeTileAvailable_ThrowInvalidOperation()
  {
    //Arange
    byte playerAllegiance = 1;

    Home home = null!;

    Player player = new Player
    {
      PlayerNr = playerAllegiance,
      InPlay = true,
      Pieces = [],
      Home = home,
    };

    home = new()
    {
      Owner = player,
      HomeTiles = GetHomeTiles(4, player)
    };

    //Assert
    home.Invoking(obj => obj.GetFirstAvailableHomeTile())
      .Should()
      .Throw<InvalidOperationException>()
      .WithMessage("Could not find valid HomeTile for home move, please check home tile to piece ratio");
  }

  public static IEnumerable<object?[]> GetSetupAvailableSpace(byte playerAllegiance, int testCount = 1)
  {
    IEnumerable<object?[]> data = Enumerable
      .Range(0, testCount)
      .Select(_ => {
        Home home = null!;
        Player player = new Player
        {
          PlayerNr = playerAllegiance,
          InPlay = true,
          Pieces = new Piece[4],
          Home = home,
        };

        DateTime dt = DateTime.Now;
        Random rnd = new Random((int)(dt.Ticks * dt.Millisecond));

        int filedTiles = rnd.Next(3) + 1;
        HomeTile[] tiles = GetHomeTiles(filedTiles, player);

        home = new Home
        {
          HomeTiles = tiles,
          Owner = player
        };

        HomeTile expectedResult = home
          .HomeTiles
          .First(tile => !tile.Pieces.Any());

        return new object?[] {
          home,
          expectedResult
        };
      });

    return data;
  }

  private static HomeTile[] GetHomeTiles(int filledTiles, Player owner)
  {
    StandardTile exitLocation = new StandardTile
    {
      IndexInBoard = 0,
      Pieces = [],
      NextTile = null!,
    };

    HomeTile[] tiles = Enumerable
      .Range(0, 4)
      .Select(_ => new HomeTile {
        IndexInBoard = 1,
        Pieces = new(),
        PlayerNr = owner.PlayerNr,
        NextTile = exitLocation
      }).
      ToArray();

    DateTime dt = DateTime.Now;

    Random rnd = new Random((int)(dt.Ticks * dt.Millisecond));

    for (int i = 0; i < filledTiles; ++i)
    {
      IEnumerable<HomeTile> emptyTile = tiles.Where(tile => !tile.Pieces.Any());
      int noNextEmptyTile = rnd.Next(emptyTile.Count());

      HomeTile tile = emptyTile.Skip(noNextEmptyTile).First();
      tile.Pieces.Add(new Piece
      {
        CurrentTile = tile,
        Owner = owner,
        PieceState = PieceState.Home
      });
    }

    return tiles;
  }
}

