using Microsoft.EntityFrameworkCore;

using J3.Models;
using J3.Data;

namespace J3.Routes;

public static class ColourRoutes
{
    // This method maps all colour-related routes to the application
    public static void MapColourRoutes(this WebApplication app)
    {
        // GET /colours - Retrieves a list of all colours from the database
        app.MapGet(
            "/colours",
            async (ApplicationDbContext context) =>
            {
                // Asynchronously fetches all colours from the Colours table in the database
                var colours = await context.Colours.ToListAsync();

                // Returns the list of colours as a successful HTTP response with a 200 status code
                return Results.Ok(colours);
            }
        );

        // GET /colours/{id} - Retrieves a specific colour by its ID
        app.MapGet(
            "/colours/{id}",
            async (int id, ApplicationDbContext context) =>
            {
                // Attempts to find the colour by its ID in the Colours table
                var colour = await context.Colours.FindAsync(id);

                // If the colour is not found, return a 404 Not Found response
                if (colour == null)
                {
                    return Results.NotFound($"Colour with ID {id} not found.");
                }

                // If the colour is found, return the colour in the response
                return Results.Ok(colour);
            }
        );

        // POST /colours - Creates a new colour and saves it to the database
        app.MapPost(
            "/colours",
            // Model binding: ASP.NET Core automatically converts the incoming JSON data into an instance of the Colour class, based on matching property names.
            async (Colour colour, ApplicationDbContext context) =>
            {
                // Adds the new Colour object to the context (which tracks changes)
                context.Colours.Add(colour);

                // Saves all changes to the database (inserts the new colour)
                await context.SaveChangesAsync();

                // Returns a 201 Created response with the location of the newly created colour and the colour data
                return Results.Created($"/colours/{colour.Id}", colour);
            }
        );
    }
}
