using Ludo.Common.Models.Tiles;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace Ludo.Blazor.Components;

public partial class TileComponent : ComponentBase
{
  [Parameter, EditorRequired]
  public TileBase? Tile { get; set; }

  [Parameter, EditorRequired]
  public required int TileRow { get; set; }

  [Parameter, EditorRequired]
  public required int TileColumn { get; set; }

  [Parameter]
  public EventCallback<(int, int, TileBase?)> GetTile { get; set; }

  private async Task GetTileAsync()
  {
    await GetTile.InvokeAsync((TileRow, TileColumn, Tile));
  }

  private string GetTileStyle()
  {
    StringBuilder sb = new();

    if (Tile is null)
      return string.Empty;

    if (Tile.PlayerNr is not null)
      sb.Append($"player-{Tile.PlayerNr}");

    return sb.ToString();
  }

}