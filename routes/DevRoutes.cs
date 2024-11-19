using J3.Data;
using J3.Models;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class DevRoutes
{
    public static void MapDevRoutes(this WebApplication app)
    {
        app.MapGet("/dev/collections/user/{userId}",
            async (string userId, ColourContext context) =>
            {
                var user = await context.Users.FindAsync(userId);
                if (user == null)
                {
                    return Results.NotFound($"User with ID {userId} not found.");
                }

                var collections = await context.Collections
                    .Where(c => c.UserId == userId)
                    .Include(c => c.User)
                    .Include(c => c.ColourCollections)
                        .ThenInclude(cc => cc.Colour)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                if (!collections.Any())
                {
                    return Results.NotFound($"No collections found for user {userId}.");
                }
                else
                {
                    return Results.Ok(collections);
                }
            }
        ).WithTags("Dev");

        app.MapPost("/dev/colours/seed",
            async (ColourContext context) =>
            {
                var colours = new List<Colour>();
                var random = new Random();
                
                for (int h = 0; h < 360; h += 360/300)
                {
                    double s = 0.7 + (random.NextDouble() * 0.3);
                    double v = 0.7 + (random.NextDouble() * 0.3);
                    
                    double c = v * s;
                    double x = c * (1 - Math.Abs(((h / 60.0) % 2) - 1));
                    double m = v - c;
                    
                    double r, g, b;

                    if (h < 60) { r = c; g = x; b = 0; }
                    else if (h < 120) { r = x; g = c; b = 0; }
                    else if (h < 180) { r = 0; g = c; b = x; }
                    else if (h < 240) { r = 0; g = x; b = c; }
                    else if (h < 300) { r = x; g = 0; b = c; }
                    else { r = c; g = 0; b = x; }
                    
                    // Convert to hex
                    string hex = String.Format("{0:X2}{1:X2}{2:X2}", 
                        (int)((r + m) * 255), 
                        (int)((g + m) * 255), 
                        (int)((b + m) * 255)
                        );
                    
                    colours.Add(new Colour { Hex = hex });
                }
        
                await context.Colours.AddRangeAsync(colours);
                await context.SaveChangesAsync();
        
                return Results.Ok(new { 
                    message = $"Successfully seeded {colours.Count} colours",
                    colours = colours 
                });
            }
        ).WithTags("Dev");
    }
}
