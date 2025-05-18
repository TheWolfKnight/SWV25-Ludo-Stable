namespace Ludo.Blazor.Models
{
  public class PlayerColorMap
  {
    private Dictionary<byte, string> _colorMap = new();

    public void ClearColorMap()
    {
      _colorMap.Clear();
    }

    public void AddPlayerColor(byte playerNr, string color)
    {
      _colorMap.Add(playerNr, color);
    }

    public string GetPlayerColor(byte playerNr)
    {
      return _colorMap[playerNr];
    }

    public void MakeDefaultColorMap()
    {
      _colorMap = new()
      {
        { 0, "red" },
        { 1, "blue" },
        { 2, "green" },
        { 3, "yellow" }
      };
    }
  }
}
