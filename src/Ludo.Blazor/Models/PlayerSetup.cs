namespace Ludo.Blazor.Models;

public class PlayerSetup
{
    public required int PlayerNr { get; set; }
    public required bool CanRoll { get; set; }
    public int? Roll { get; set; }
}