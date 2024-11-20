namespace J3.Utils;

public static class Utilities
{
    public static (int R, int G, int B) HexToRgb(string hex)
    {
        if (string.IsNullOrEmpty(hex))
        {
            throw new ArgumentNullException(nameof(hex));
        }
        
        hex = hex.TrimStart('#');
        
        if (!System.Text.RegularExpressions.Regex.IsMatch(hex, "^[0-9A-Fa-f]{6}$"))
        {
            throw new ArgumentException("Invalid hex color format", nameof(hex));
        }
    
        try
        {
            int r = Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = Convert.ToInt32(hex.Substring(4, 2), 16);
            
            return (r, g, b);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Failed to parse hex color", nameof(hex), ex);
        }
    }

    public static (double H, double S, double L) RgbToHsl(int r, int g, int b)
    {
        double rd = r / 255.0;
        double gd = g / 255.0;
        double bd = b / 255.0;
    
        double max = Math.Max(rd, Math.Max(gd, bd));
        double min = Math.Min(rd, Math.Min(gd, bd));
        double delta = max - min;
    
        double l = (max + min) / 2.0;
    
        double h = 0;
        double s = 0;
    
        if (delta != 0)
        {
            s = l < 0.5 ?
                delta / (max + min) :
                delta / (2.0 - max - min);

            if (max == rd)
            {
                h = ((gd - bd) / delta) + (gd < bd ? 6 : 0);
            }
            else if (max == gd)
            {
                h = ((bd - rd) / delta) + 2;
            }
            else
            {
                h = ((rd - gd) / delta) + 4;
            }
            h /= 6;
        }
    
        h *= 360;
    
        return (h, s, l);
    }

    public static string PickColours(double hue, double saturation, double lightness)
    {
        List<(string Name, double Start, double End)> colorRanges = new List<(string, double, double)>
        {
            ("Red", 345, 15),
            ("Orange", 15, 45),
            ("Yellow", 45, 75),
            ("Green", 75, 165),
            ("Cyan", 165, 195),
            ("Blue", 195, 255),
            ("Purple", 255, 285),
            ("Magenta", 285, 345)
        };
    
        hue = hue < 0 ? hue + 360 : hue;
        hue = hue >= 360 ? hue - 360 : hue;
    
        var possibleColors = new List<string>();
    
        foreach (var (Name, Start, End) in colorRanges)
        {
            bool inRange = false;

            if (Start < End)
                inRange = hue >= Start && hue < End;
            else
                inRange = hue >= Start || hue < End;
    
            if (inRange)
                { possibleColors.Add(Name); }
            else if (Math.Abs(hue - Start) < 10 || Math.Abs(hue - End) < 10)
                { possibleColors.Add(Name); }
        }
    
        return string.Join("/", possibleColors);
    }

    public static string CompareShade(double saturation, double lightness)
    {
        if (lightness < 0.2) return "Dark ";
        else if (lightness > 0.8) return "Light ";
        else if (saturation < 0.25) return "Grayish ";
        return "";
    }

    public static string CompareColours(int r, int g, int b, string name)
    {
        if (name != "red" && r > g && r > b)
            return "reddish ";
        if (name != "green" && g > b && g > r)
            return "greenish ";
        if (name != "blue" && b > r && b > g)
            return "blueish ";
        return "";
    }

    public static string HexToName(string hex)
    {
        var (r, g, b) = HexToRgb(hex);
        var (hue, saturation, lightness) = RgbToHsl(r, g, b);
        
        string[] baseColors = PickColours(hue, saturation, lightness).Split('/');
        
        string shade = CompareShade(saturation, lightness);
        
        var modifiedColors = baseColors
            .Select(color => {
                string ish = CompareColours(r, g, b, color.ToLower());
                
                return $"{shade}{ish}{color.Trim()}";
            })
            .Where(c => !string.IsNullOrWhiteSpace(c));
        
        return string.Join(", ", modifiedColors);
    }
}