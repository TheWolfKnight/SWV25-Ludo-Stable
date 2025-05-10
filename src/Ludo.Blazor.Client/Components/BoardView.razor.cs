using Ludo.Common.Dtos;
using Ludo.Common.Models;
using Ludo.Common.Models.Tiles;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Client.Components
{
  public partial class BoardView : ComponentBase
  {
    [Parameter]
    public required Board Board { get; set; }

    private void SetCurrentSelectedTile((int row, int col, TileBase? tile) eventTuple)
    {
      

      StateHasChanged();
    }
  }
}
