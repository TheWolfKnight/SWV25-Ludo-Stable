using Ludo.Common.Enums;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Models.Player;

public class Piece
{
  public required MovementTile CurrentTile;
  public required Player Owner { get; init; }
  public required PieceState PieceState { get; set; }

  public void MoveToHome()
  {
    HomeTile home = Owner.Home.GetFirstAvailableHomeTile();
    home.SendPieceHome(this);
  }
}
