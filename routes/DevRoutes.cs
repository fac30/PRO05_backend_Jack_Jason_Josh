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
    }
}
