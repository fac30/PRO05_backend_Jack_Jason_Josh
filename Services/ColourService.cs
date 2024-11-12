using System.Text.Json;
using J3.Models;

namespace J3.Services;

public static class ColourService
{
    public static List<Colour> LoadColoursFromJson(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var colourData = JsonSerializer.Deserialize<ColourData>(json);
        return colourData?.Rows ?? new List<Colour>();
    }
}
