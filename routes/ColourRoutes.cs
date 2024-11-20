using System.Text.Json;
using J3.Data;
using J3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class ColourRoutes
{
    public static void MapColourRoutes(this WebApplication app)
    {
        /* -------------- GET -------------- */
        app.MapGet(
                "/colours",
                async (ColourContext context) =>
                {
                    var colours = await context.Colours.ToListAsync();

                    return Results.Ok(colours);
                }
            )
            .WithTags("Colours");

        app.MapGet(
                "/colours/{id}",
                async (int id, ColourContext context) =>
                {
                    var colour = await context.Colours.FindAsync(id);

                    if (colour == null)
                    {
                        return Results.NotFound($"Colour with ID {id} not found.");
                    }

                    return Results.Ok(colour);
                }
            )
            .WithTags("Colours");

        /* -------------- POST -------------- */

        app.MapPost(
                "/colours",
                async (Colour colour, ColourContext context) =>
                {
                    context.Colours.Add(colour);
                    await context.SaveChangesAsync();

                    await context.SaveChangesAsync(); // Saves changes to database

                    return Results.Created($"/colours/{colour.Id}", colour);
                }
            )
            .WithTags("Colours");

        /* -------------- GET COLOUR NAME -------------- */


        // Minimal API Route to fetch color name based on hex code
        app.MapGet(
                "/color-name/{hex}",
                async (string hex, [FromServices] HttpClient client) =>
                {
                    try
                    {
                        var response = await client.GetAsync(
                            $"https://www.thecolorapi.com/id?hex={hex}"
                        );

                        // Ensure the request was successful
                        response.EnsureSuccessStatusCode();

                        // Read and parse the response content
                        var responseData = await response.Content.ReadAsStringAsync();
                        var jsonData = JsonDocument.Parse(responseData);

                        // Extract the color name from the response
                        var colorName = jsonData
                            .RootElement.GetProperty("name")
                            .GetProperty("value")
                            .GetString();

                        return Results.Ok(new { Hex = hex, ColorName = colorName });
                    }
                    catch
                    {
                        return Results.BadRequest("Invalid hex code or failed to fetch data.");
                    }
                }
            )
            .WithTags("Colours");
    }
}
