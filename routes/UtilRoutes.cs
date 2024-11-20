using J3.Data;
using J3.Models;
using J3.Utils;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class UtilRoutes
{
    public static void MapUtilRoutes(this WebApplication app)
    {
        app.MapGet("/search/colourhex", (String hex) => 
        {
            try
            {
                string name = ColourSearch.HexToName(hex);
                return Results.Ok(name);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).WithTags("Utils");

        app.MapGet("/search/colourname", (String name) =>
        {
            try
            {
                string[] hexArray = ColourSearch.NameToHexArray(name);
                return Results.Ok(new { hexCodes = hexArray });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).WithTags("Utils");
    }
}