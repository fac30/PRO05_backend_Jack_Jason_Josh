namespace J3.Utils;

public static class ColourSearch
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


    /* ---------------SEARCH BY NAME--------------- */

    public static string[] NameToHexArray(string colorName)
    {
        // Get center color (index 2)
        string centerHex = NameToHex(colorName);
        var rgb = HexToRgb(centerHex);
        var (h, s, l) = RgbToHsl(rgb.R, rgb.G, rgb.B);
        
        string[] hexArray = new string[5];
        
        hexArray[2] = centerHex;

        // Min/Max Values
        hexArray[0] = HslToHex(h, Math.Max(0, s - 0.2), Math.Max(0, l - 0.2));
        hexArray[4] = HslToHex(h, Math.Min(1, s + 0.2), Math.Min(1, l + 0.2));

        // Intermediate Values
        hexArray[1] = InterpolateHex(hexArray[0], hexArray[2]);
        hexArray[3] = InterpolateHex(hexArray[2], hexArray[4]);
        
        return hexArray;
    }

    public static string NameToHex(string colorName)
    {
        string[] parts = colorName
            .ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        double hue = 0;
        double saturation = 0.7;  // Default moderate saturation
        double lightness = 0.5;   // Default medium lightness
        
        string baseColor = parts[^1];

        // Handle brown edge cases
        if (baseColor == "brown")
        {
            baseColor = "orange";
            var hasLight = parts.Take(parts.Length - 1).Contains("light");
            var hasDark = parts.Take(parts.Length - 1).Contains("dark");

            if (!hasLight && !hasDark)
            {
                parts = parts.Take(parts.Length - 1).Append("dark").Append(baseColor).ToArray();
            }
            else if (hasLight)
            {
                parts = parts.Take(parts.Length - 1)
                    .Where(p => p != "light")
                    .Append(baseColor)
                    .ToArray();
            }
            else if (hasDark)
            {
                parts = parts.Take(parts.Length - 1)
                    .Append("dark")
                    .Append(baseColor)
                    .ToArray();
            }
        }

        // Handle pink edge cases
        if (baseColor == "pink")
        {
            baseColor = "red";
            var hasLight = parts.Take(parts.Length - 1).Contains("light");
            var hasDark = parts.Take(parts.Length - 1).Contains("dark");

            if (!hasLight && !hasDark)
            {
                parts = parts.Take(parts.Length - 1)
                    .Append("light")
                    .Append(baseColor)
                    .ToArray();
            }
            else if (hasLight)
            {
                parts = parts.Take(parts.Length - 1)
                    .Append("light")
                    .Append(baseColor)
                    .ToArray();
            }
            else if (hasDark)
            {

                parts = parts.Take(parts.Length - 1).Append(baseColor).ToArray();
            }
        }

        var colorRange = GetColorRange(baseColor);
        hue = (colorRange.Start + colorRange.End) / 2;
        
        foreach (string modifier in parts.Take(parts.Length - 1))
        {
            switch (modifier)
            {
                case "light":
                    lightness += 0.15;
                    if (lightness == 1) lightness = 0.95;
                    break;
                case "dark":
                    lightness -= 0.15;
                    if (lightness == 0) lightness = 0.05;
                    break;
                case "pale":
                    saturation -= 0.2;
                    break;
                case "bold":
                    saturation += 0.2;
                    break;
                case "reddish":
                    hue = AdjustHueTowards(hue, 0);
                    break;
                case "greenish":
                    hue = AdjustHueTowards(hue, 120);
                    break;
                case "blueish":
                    hue = AdjustHueTowards(hue, 240);
                    break;
            }
        }
        
        return HslToHex(hue, saturation, lightness);
    }

    private static (double Start, double End) GetColorRange(string colorName)
    {
        Dictionary<string, (double Start, double End)> colorRanges = new()
        {
            {"red", (345, 15)},
            {"orange", (15, 45)},
            {"yellow", (45, 75)},
            {"green", (75, 165)},
            {"cyan", (165, 195)},
            {"blue", (195, 255)},
            {"purple", (255, 285)},
            {"magenta", (285, 345)}
        };

        if (!colorRanges.ContainsKey(colorName))
            throw new ArgumentException($"Unknown color name: {colorName}");

        return colorRanges[colorName];
    }

    private static double AdjustHueTowards(double currentHue, double targetHue)
    {
        // Calculate both clockwise and counterclockwise distances
        double diff = (targetHue - currentHue + 360) % 360;
        double distance = Math.Min(diff, 360 - diff);
        
        // Move 20% of the way towards the target
        return (currentHue + (distance * 0.2)) % 360;
    }

    private static string HslToHex(double h, double s, double l)
    {
        // First convert HSL to RGB
        double c = (1 - Math.Abs(2 * l - 1)) * s;
        double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
        double m = l - c / 2;

        double r, g, b;

        if (h < 60) { r = c; g = x; b = 0; }
        else if (h < 120) { r = x; g = c; b = 0; }
        else if (h < 180) { r = 0; g = c; b = x; }
        else if (h < 240) { r = 0; g = x; b = c; }
        else if (h < 300) { r = x; g = 0; b = c; }
        else { r = c; g = 0; b = x; }

        // Convert to hex
        return String.Format("{0:X2}{1:X2}{2:X2}",
            (int)((r + m) * 255),
            (int)((g + m) * 255),
            (int)((b + m) * 255)
        );
    }

    private static string InterpolateHex(string hex1, string hex2)
    {
        var (r1, g1, b1) = HexToRgb(hex1);
        var (r2, g2, b2) = HexToRgb(hex2);

        // Calculate the average RGB values
        int r = (r1 + r2) / 2;
        int g = (g1 + g2) / 2;
        int b = (b1 + b2) / 2;

        // Convert back to hex
        return String.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
    }
}