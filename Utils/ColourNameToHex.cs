// namespace J3.Utils;

// public static class Utilities
// {
//     public static string[] NameToHexArray(string colorName)
//     {
//         // Get center color (index 2)
//         string centerHex = NameToHex(colorName);
//         var (h, s, l) = RgbToHsl(HexToRgb(centerHex));
        
//         string[] hexArray = new string[5];
        
//         hexArray[2] = centerHex;

//         // Min/Max Values
//         hexArray[0] = HslToHex(h, Math.Max(0, s - 0.2), Math.Max(0, l - 0.2));
//         hexArray[4] = HslToHex(h, Math.Min(1, s + 0.2), Math.Min(1, l + 0.2));

//         // Intermediate Values
//         hexArray[1] = InterpolateHex(hexArray[0], hexArray[2]);
//         hexArray[3] = InterpolateHex(hexArray[2], hexArray[4]);
        
//         return hexArray;
//     }

//     public static string NameToHex(string colorName)
//     {
//         string[] parts = colorName
//             .ToLower()
//             .Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
//         double hue = 0;
//         double saturation = 0.7;  // Default moderate saturation
//         double lightness = 0.5;   // Default medium lightness
        
//         string baseColor = parts[^1];
//         var colorRange = GetColorRange(baseColor);
//         hue = (colorRange.Start + colorRange.End) / 2;
        
//         foreach (string modifier in parts.Take(parts.Length - 1))
//         {
//             switch (modifier)
//             {
//                 case "light":
//                     lightness = 0.7;
//                     break;
//                 case "dark":
//                     lightness = 0.3;
//                     break;
//                 case "reddish":
//                     hue = AdjustHueTowards(hue, 0);
//                     break;
//                 case "greenish":
//                     hue = AdjustHueTowards(hue, 120);
//                     break;
//                 case "blueish":
//                     hue = AdjustHueTowards(hue, 240);
//                     break;
//             }
//         }
        
//         return HslToHex(hue, saturation, lightness);
//     }

//     private static (double Start, double End) GetColorRange(string colorName)
//     {
//         Dictionary<string, (double Start, double End)> colorRanges = new()
//         {
//             {"red", (345, 15)},
//             {"orange", (15, 45)},
//             {"yellow", (45, 75)},
//             {"green", (75, 165)},
//             {"cyan", (165, 195)},
//             {"blue", (195, 255)},
//             {"purple", (255, 285)},
//             {"magenta", (285, 345)}
//         };

//         if (!colorRanges.ContainsKey(colorName))
//             throw new ArgumentException($"Unknown color name: {colorName}");

//         return colorRanges[colorName];
//     }

//     private static double AdjustHueTowards(double currentHue, double targetHue)
//     {
//         // Calculate both clockwise and counterclockwise distances
//         double diff = (targetHue - currentHue + 360) % 360;
//         double distance = Math.Min(diff, 360 - diff);
        
//         // Move 20% of the way towards the target
//         return (currentHue + (distance * 0.2)) % 360;
//     }

//     private static string HslToHex(double h, double s, double l)
//     {
//         // First convert HSL to RGB
//         double c = (1 - Math.Abs(2 * l - 1)) * s;
//         double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
//         double m = l - c / 2;

//         double r, g, b;

//         if (h < 60) { r = c; g = x; b = 0; }
//         else if (h < 120) { r = x; g = c; b = 0; }
//         else if (h < 180) { r = 0; g = c; b = x; }
//         else if (h < 240) { r = 0; g = x; b = c; }
//         else if (h < 300) { r = x; g = 0; b = c; }
//         else { r = c; g = 0; b = x; }

//         // Convert to hex
//         return String.Format("{0:X2}{1:X2}{2:X2}",
//             (int)((r + m) * 255),
//             (int)((g + m) * 255),
//             (int)((b + m) * 255)
//         );
//     }
// }
