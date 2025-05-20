using System.Reflection;
using Ludo.Common.Enums;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Dtos;

public record TileDto
{
  public required TileTypes Type { get; init; }

  public required Dictionary<string, object?> Data { get; init; } = [];

  public static TileDto FromTile(TileBase tileBase)
  {
    return new TileDto()
    {
      Type = GetTileType(tileBase),
      Data = GetTileData(tileBase)
    };
  }

  private static Dictionary<string, object?> GetTileData(TileBase tileBase)
  {
    Type tileType = tileBase.GetType();

    PropertyInfo[] tileProperties = tileType.GetProperties();

    Dictionary<string, object?> properties = new();
    
    foreach (PropertyInfo property in tileProperties)
    {
      if(property.Name is nameof(MovementTile.Pieces))
        continue;
      
      object? propertyObject = property.GetValue(tileBase);
      
      if(propertyObject is TileBase tile)
        properties.Add(property.Name, tile.IndexInBoard);
      else
        properties.Add(property.Name, propertyObject);
    }

    return properties;
  }

  private static TileTypes GetTileType(TileBase tileBase)
  {
    Type tileType = tileBase.GetType();

    int stopIndex = tileType.Name.IndexOf("Tile", StringComparison.OrdinalIgnoreCase);

    string tileTypeName = tileType.Name[..stopIndex];

    if (!Enum.TryParse(tileTypeName, out TileTypes typeOfTile))
      throw new InvalidOperationException($"Could not parse type name {tileTypeName} to a valid type.");

    return typeOfTile;
  }
}
