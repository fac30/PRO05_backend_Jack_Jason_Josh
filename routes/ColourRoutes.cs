using J3.Data;
using J3.Models;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class ColourRoutes
{
    public static void MapColourRoutes(this WebApplication app)
    {
        app.MapGet("/colours",
            async (ColourContext context) =>
            {
                var colours = await context.Colours.ToListAsync();

                return Results.Ok(colours);
            }
        ).WithTags("Colours");

        app.MapGet("/colours/{id}",
            async (int id, ColourContext context) =>
            {
                var colour = await context.Colours.FindAsync(id);

                if (colour == null)
                {
                    return Results.NotFound($"Colour with ID {id} not found.");
                }

                return Results.Ok(colour);
            }
        ).WithTags("Colours");

        app.MapPost("/colours",
            async (Colour colour, ColourContext context) =>
            {
                context.Colours.Add(colour); // Adds new Colour object to context

                await context.SaveChangesAsync(); // Saves changes to database

                return Results.Created($"/colours/{colour.Id}", colour);
            }
        ).WithTags("Colours");
    }
}
