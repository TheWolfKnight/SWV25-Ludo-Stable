using Ludo.Blazor.Models;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace Ludo.Blazor.Components;

public partial class TileComponent : ComponentBase
{
  [Inject]
  public required PlayerColorMap ColorMap { get; set; }

  [Parameter, EditorRequired]
  public TileBase? Tile { get; set; }

  [Parameter, EditorRequired]
  public required int TileRow { get; set; }

  [Parameter, EditorRequired]
  public required int TileColumn { get; set; }

  [Parameter]
  public EventCallback<TileBase?> GetTile { get; set; }

  private async Task GetTileAsync()
  {
    await GetTile.InvokeAsync(Tile);
  }

  private string GetTileStyle()
  {
    StringBuilder sb = new();

    if (Tile?.PlayerNr is not null)
      sb.Append($"player-{ColorMap.GetPlayerColor(Tile.PlayerNr.Value)}");
    else if (Tile is StandardTile)
      sb.Append("standard-tile-no-aligance");

      return sb.ToString();
  }

  private string GetPieceImage()
  {
    if (Tile is not MovementTile movementTile || movementTile.Pieces.Count() == 0)
      return "";

    Piece piece = movementTile.Pieces.First();

    string path = "Images/Pieces/" + ColorMap.GetPlayerColor(piece.Owner.PlayerNr) + ".png";
    return path;
  }
}