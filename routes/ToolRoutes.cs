using J3.Data;
using J3.Models;
using J3.Utils;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class ToolRoutes
{
    public static void MapToolRoutes(this WebApplication app)
    {
        app.MapGet("/search/hex", (String hex) => {
            string name = Utilities.HexToName(hex);
            return Results.Ok(name);
        });
    }
}