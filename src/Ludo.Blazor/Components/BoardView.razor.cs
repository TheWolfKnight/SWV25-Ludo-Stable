using Ludo.Common.Models;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Components
{
  public partial class BoardView : ComponentBase
  {
    [Parameter, EditorRequired]
    public required EventCallback<Piece> OnMakeMove { get; set; }

    [Parameter, EditorRequired]
    public required Board Board { get; set; }

    private async Task SetCurrentSelectedTile(TileBase? tile)
    {
      if (tile is not MovementTile movementTile || movementTile.Pieces.Count == 0)
        return;

      if (OnMakeMove is EventCallback<Piece> callback)
        await callback.InvokeAsync(movementTile.Pieces.FirstOrDefault());
    }
  }
}
