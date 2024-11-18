using J3.Data;
using J3.Models;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class CollectionRoutes
{
    public static void MapCollectionRoutes(this WebApplication app)
    {
        app.MapGet("/collections",
            async (ColourContext context) =>
            {
                var collections = await context.Collections
                    .Include(c => c.User)
                    .ToListAsync();

                return Results
                    .Ok(collections);
            }
        ).WithTags("Collections");

        app.MapGet("/collections/public",
            async (ColourContext context) =>
            {
                var collections = await context.Collections
                    .Where(c => c.IsPublic)
                    .Include(c => c.User)
                    .ToListAsync();

                return Results
                    .Ok(collections);
            }
        ).WithTags("Collections");
        
        app.MapGet("/collections/favourite",
            async (ColourContext context) =>
            {
                var collections = await context.Collections
                    .Where(c => c.Type == "favourite")
                    .Include(c => c.User)
                    .ToListAsync();

                return Results
                    .Ok(collections);
            }
        ).WithTags("Collections");
                
        app.MapGet("/collections/palette",
            async (ColourContext context) =>
            {
                var collections = await context.Collections
                    .Where(c => c.Type == "palette")
                    .Include(c => c.User)
                    .ToListAsync();

                return Results
                    .Ok(collections);
            }
        ).WithTags("Collections");
        
        app.MapGet("/collections/{id}",
            async (int id, ColourContext context) =>
            {
                var collection = await context.Collections
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (collection == null)
                {
                    return Results
                        .NotFound($"Collection with ID {id} not found.");
                }

                if (!collection.IsPublic)
                {
                    return Results
                        .NotFound($"Collection with ID {id} is not public.");
                }

                return Results
                    .Ok(collection);
            }
        ).WithTags("Collections");  
    }
}