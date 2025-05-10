using System.Text;
using Microsoft.AspNetCore.Components;
using Ludo.Blazor.Client;
using Ludo.Common.Models.Tiles;

namespace Ludo.Blazor.Client.Components;

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