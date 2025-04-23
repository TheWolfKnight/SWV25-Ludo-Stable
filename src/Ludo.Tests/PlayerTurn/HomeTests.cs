using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Ludo.Tests;

public class HomeTests
{
  [Theory]
  [MemberData(nameof(GetSetupAvailableSpace), (byte)1)]
  public void Home_TryGetEmptyHome_WithHomeTileAvailable_ReturnFirstHomeTile(Home home, Player player, int piecesAtHome)
  {
    //Arange
    HomeTile[] tiles = GetHomeTiles(piecesAtHome, player.PlayerNr);
    for (int i = 0; i < tiles.Length; ++i)
      home.HomeTiles[i] = tiles[i];

    //Act
    HomeTile tile = home.GetFirstAvailableHomeTile();

    using AssertionScope scope = new();
    tile.Should().Be(home.HomeTiles[piecesAtHome]);
    tile.Pieces.Should().BeEmpty();
  }

  [Fact]
  public void Home_TryGetEmptyHome_WithoutHomeTileAvailable_ThrowInvalidOperation()
  {
    //Arange
    byte playerAllegiance = 1;

    Home home = null!;
    home = new()
    {
      Owner = new Player
      {
        PlayerNr = playerAllegiance,
        InPlay = true,
        Pieces = [],
        Home = home,
      },
      HomeTiles = GetHomeTiles(4, playerAllegiance)
    };

    //Assert
    home.Invoking(obj => obj.GetFirstAvailableHomeTile())
      .Should()
      .Throw<InvalidOperationException>()
      .WithMessage("Could not find valid HomeTile for home move, please check home tile to piece ratio");
  }

  public static IEnumerable<object?[]> GetSetupAvailableSpace(byte playerAllegiance)
  {
    Home home = null!;
    home = new()
    {
      Owner = new Player
      {
        PlayerNr = playerAllegiance,
        InPlay = true,
        Pieces = A.CollectionOfFake<Piece>(4).ToArray(),
        Home = home,
      },
      HomeTiles = new HomeTile[4]
    };

    Player player = home.Owner;

    IEnumerable<object?[]> data = [
      [
        home,
        player,
        0
      ],
      [
        home,
        player,
        1
      ],
      [
        home,
        player,
        2
      ],
      [
        home,
        player,
        3
      ]
    ];

    return data;
  }

  public static HomeTile[] GetHomeTiles(int filledTiles, byte playerAllegiance)
  {
    StandardTile exitLocation = new StandardTile
    {
      Location = (1, 1),
      Pieces = [],
      NextTile = null!,
    };

    IEnumerable<HomeTile> empty = Enumerable
      .Range(0, 4 - filledTiles)
      .Select(_ =>
        new HomeTile
        {
          Location = (1, 1),
          Pieces = [],
          NextTile = exitLocation,
          PlayerNr = playerAllegiance
        }
      );

    IEnumerable<HomeTile> filled = Enumerable
      .Range(0, filledTiles)
      .Select(_ =>
        new HomeTile
        {
          Location = (1, 1),
          Pieces = [A.Fake<Piece>()],
          NextTile = exitLocation,
          PlayerNr = playerAllegiance
        }
      );

    return empty
      .Concat(filled)
      .ToArray();
  }
}

