using J3.Data;
using J3.Models;
using J3.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class CollectionRoutes
{
    public static void MapCollectionRoutes(this WebApplication app)
    {
        /* -------------- GET -------------- */

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

        /* -------------- POST -------------- */

        app.MapPost("/collections",
            async (CreateCollectionDTO dto, ColourContext context) =>
            {
                var validationErrors = new List<string>();
                
                /* Check Name */ if (string.IsNullOrEmpty(dto.Name))
                {
                    validationErrors.Add("Collection name is required.");
                }
                
                /* Check Type */ if (string.IsNullOrEmpty(dto.Type))
                {
                    validationErrors.Add("Collection type is required.");
                }
                else if (!new[] { "palette", "favourite" }.Contains(dto.Type))
                {
                    validationErrors.Add("Collection type must be either 'palette' or 'favourite'.");
                }
                
                /* Check User */ if (dto.UserId == null)
                {
                    validationErrors.Add("User ID is required.");
                }
                else 
                {
                    // Check the user exists
                    var user = await context.Users.FindAsync(dto.UserId);
                    if (user == null)
                    {
                        validationErrors.Add($"User with ID {dto.UserId} not found.");
                    }
                }

                /* Any Errors? */ if (validationErrors.Any())
                {
                    return Results.BadRequest(new { Errors = validationErrors });
                }

                var collection = new Collection
                {
                    Name = dto.Name!,
                    Description = dto.Description,
                    Type = dto.Type!,
                    IsPublic = dto.IsPublic,
                    UserId = dto.UserId!.Value
                };

                context.Collections.Add(collection);
                await context.SaveChangesAsync();

                return Results.Created($"/collections/{collection.Id}", collection);
            }
        ).WithTags("Collections");

        /* -------------- PUT -------------- */

        app.MapPut("/collections/{id}/privacy",
            async (int id, ColourContext context) =>
            {
                var collection = await context.Collections.FindAsync(id);
                
                if (collection == null)
                {
                    return Results.NotFound($"Collection with ID {id} not found.");
                }

                collection.IsPublic = !collection.IsPublic;
                await context.SaveChangesAsync();

                return Results.Ok(collection);
            }
        ).WithTags("Collections");
    }
}