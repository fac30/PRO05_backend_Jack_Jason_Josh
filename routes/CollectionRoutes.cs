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

        app.MapGet("/collections/{id}/colours",
            async (int id, ColourContext context) =>
            {
                var collection = await context.Collections
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (collection == null)
                {
                    return Results.NotFound($"Collection with ID {id} not found.");
                }

                var colourCollections = await context.ColourCollections
                    .Where(cc => cc.CollectionId == id)
                    .Include(cc => cc.Colour)
                    .OrderBy(cc => cc.Order)
                    .ToListAsync();

                var result = new
                {
                    Collection = collection,
                    Colours = colourCollections.Select(cc => new
                    {
                        Id = cc.Colour!.Id,
                        Hex = cc.Colour.Hex,
                        Order = cc.Order
                    })
                };

                return Results.Ok(result);
            }
        ).WithTags("Collections (Colours)");

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

        app.MapPost("/collections/{id}/colours",
            async (int id, AddColourToCollectionDTO dto, ColourContext context) =>
            {
                // Check collection exists
                var collection = await context.Collections.FindAsync(id);
                if (collection == null)
                {
                    return Results.NotFound($"Collection with ID {id} not found.");
                }

                // Check colour exists
                var colour = await context.Colours.FindAsync(dto.ColourId);
                if (colour == null)
                {
                    return Results.NotFound($"Colour with ID {dto.ColourId} not found.");
                }

                // Check if colour is already in collection
                var existingEntry = await context.ColourCollections
                    .FirstOrDefaultAsync(cc => 
                        cc.CollectionId == id && 
                        cc.ColourId == dto.ColourId);
                
                if (existingEntry != null)
                {
                    return Results.BadRequest($"Colour {dto.ColourId} is already in collection {id}.");
                }

                var colourCollection = new ColourCollection
                {
                    CollectionId = id,
                    ColourId = dto.ColourId,
                    Order = dto.Order
                };

                context.ColourCollections.Add(colourCollection);
                await context.SaveChangesAsync();

                return Results.Created($"/collections/{id}/colours", colourCollection);
            }
        ).WithTags("Collections (Colours)");

        /* -------------- PUT -------------- */

        app.MapPut("/collections/{id}/text",
            async (int id, UpdateCollectionDTO dto, ColourContext context) =>
            {
                var collection = await context.Collections.FindAsync(id);
                
                if (collection == null)
                {
                    return Results.NotFound($"Collection with ID {id} not found.");
                }

                // Validate and update name if provided
                if (dto.Name != null)
                {
                    if (string.IsNullOrEmpty(dto.Name.Trim()))
                    {
                        return Results.BadRequest("Collection name cannot be empty.");
                    }
                    collection.Name = dto.Name;
                }

                // Update description if provided (can be null)
                if (dto.Description != null)
                {
                    collection.Description = dto.Description;
                }

                await context.SaveChangesAsync();
                return Results.Ok(collection);
            }
        ).WithTags("Collections");

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

        app.MapPut("/collections/{id}/type",
            async (int id, string type, ColourContext context) =>
            {
                var collection = await context.Collections.FindAsync(id);
                
                if (collection == null)
                {
                    return Results.NotFound($"Collection with ID {id} not found.");
                }

                if (!new[] { "palette", "favourite" }.Contains(type))
                {
                    return Results.BadRequest("Collection type must be either 'palette' or 'favourite'.");
                }

                collection.Type = type;
                await context.SaveChangesAsync();

                return Results.Ok(collection);
            }
        ).WithTags("Collections");

        app.MapPut("/collections/{collectionId}/colours/{colourId}/order",
            async (int collectionId, int colourId, int newOrder, ColourContext context) =>
            {
                // Get all colours in the collection to validate the new order
                var collectionColours = await context.ColourCollections
                    .Where(cc => cc.CollectionId == collectionId)
                    .OrderBy(cc => cc.Order)
                    .ToListAsync();

                if (!collectionColours.Any())
                {
                    return Results.NotFound($"Collection {collectionId} has no colours.");
                }

                var maxOrder = collectionColours.Count - 1;
                if (newOrder < 0 || newOrder > maxOrder)
                {
                    return Results.BadRequest($"Order must be between 0 and {maxOrder}.");
                }

                var colourToMove = collectionColours
                    .FirstOrDefault(cc => cc.ColourId == colourId);

                if (colourToMove == null)
                {
                    return Results.NotFound($"Colour {colourId} not found in collection {collectionId}.");
                }

                var oldOrder = colourToMove.Order;

                // If moving to same position, no change needed
                if (oldOrder == newOrder)
                {
                    return Results.Ok(colourToMove);
                }

                // Reorder colours between old and new positions
                if (newOrder > oldOrder)
                {
                    // Moving down - shift others up
                    foreach (var colour in collectionColours)
                    {
                        if (colour.Order > oldOrder && colour.Order <= newOrder)
                        {
                            colour.Order--;
                        }
                    }
                }
                else
                {
                    // Moving up - shift others down
                    foreach (var colour in collectionColours)
                    {
                        if (colour.Order >= newOrder && colour.Order < oldOrder)
                        {
                            colour.Order++;
                        }
                    }
                }

                colourToMove.Order = newOrder;
                await context.SaveChangesAsync();

                return Results.Ok(colourToMove);
            }
        ).WithTags("Collections (Colours)");

        /* -------------- DELETE -------------- */

        app.MapDelete("/collections/{id}",
            async (int id, ColourContext context) =>
            {
                var collection = await context.Collections
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (collection == null)
                {
                    return Results.NotFound($"Collection with ID {id} not found.");
                }

                // Note: We don't need to explicitly delete ColourCollections or Comments
                // because we set up CASCADE DELETE in our migrations
                context.Collections.Remove(collection);
                await context.SaveChangesAsync();

                return Results.Ok($"Collection '{collection.Name}' and all associated data deleted.");
            }
        ).WithTags("Collections");
                
        app.MapDelete("/collections/{collectionId}/colours/{colourId}",
            async (int collectionId, int colourId, ColourContext context) =>
            {
                var colourCollection = await context.ColourCollections
                    .FirstOrDefaultAsync(cc => 
                        cc.CollectionId == collectionId && 
                        cc.ColourId == colourId);

                if (colourCollection == null)
                {
                    return Results.NotFound($"Colour {colourId} not found in collection {collectionId}.");
                }

                // Get the order of the colour being removed
                int removedOrder = colourCollection.Order;

                // Remove the colour
                context.ColourCollections.Remove(colourCollection);

                // Get all colours with higher order values
                var higherOrderColours = await context.ColourCollections
                    .Where(cc => 
                        cc.CollectionId == collectionId && 
                        cc.Order > removedOrder)
                    .ToListAsync();

                // Decrease their order by 1
                foreach (var colour in higherOrderColours)
                {
                    colour.Order--;
                }

                await context.SaveChangesAsync();

                return Results.Ok();
            }
        ).WithTags("Collections (Colours)");
    }
}