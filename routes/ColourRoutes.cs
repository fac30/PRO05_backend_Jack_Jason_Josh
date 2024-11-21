using System.Text.Json;
using J3.ColourExtensions;
using J3.Data;
using J3.Models;
using J3.Models.DTOs;
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
                async (
                    CreateColourDTO dto,
                    ColourContext context,
                    ColourNameExtensions getColourName
                ) =>
                {
                    var colourName = await getColourName.GetColorNameAsync(dto.Hex);

                    if (colourName == null)
                    {
                        return Results.BadRequest(
                            "Invalid hex code or failed to fetch color name."
                        );
                    }

                    // Set the color name for the Colour object
                    var colour = new Colour { ColourName = colourName, Hex = dto.Hex };

                    context.Colours.Add(colour);
                    await context.SaveChangesAsync();

                    return Results.Created($"/colours/{colour.Id}", colour);
                }
            )
            .WithTags("Colours");

        // /* -------------- GET COLOUR NAME -------------- */


        // // Minimal API Route to fetch color name based on hex code
        // app.MapGet(
        //         "/color-name/{hex}",
        //         async (string hex, [FromServices] HttpClient client) =>
        //         {
        //             try
        //             {
        //                 var response = await client.GetAsync(
        //                     $"https://www.thecolorapi.com/id?hex={hex}"
        //                 );

        //                 // Ensure the request was successful
        //                 response.EnsureSuccessStatusCode();

        //                 // Read and parse the response content
        //                 var responseData = await response.Content.ReadAsStringAsync();
        //                 var jsonData = JsonDocument.Parse(responseData);

        //                 // Extract the color name from the response
        //                 var colorName = jsonData
        //                     .RootElement.GetProperty("name")
        //                     .GetProperty("value")
        //                     .GetString();

        //                 return Results.Ok(new { Hex = hex, ColorName = colorName });
        //             }
        //             catch
        //             {
        //                 return Results.BadRequest("Invalid hex code or failed to fetch data.");
        //             }
        //         }
        //     )
        //     .WithTags("Colours");
    }
}
