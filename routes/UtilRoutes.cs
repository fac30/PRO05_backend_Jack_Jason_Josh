using J3.Data;
using J3.Models;
using J3.Utils;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class UtilRoutes
{
    public static void MapUtilRoutes(this WebApplication app)
    {
        app.MapGet("/search/hex", (String hex) => {
            try
            {
                string name = Utilities.HexToName(hex);
                return Results.Ok(name);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).WithTags("Utils");
    }
}