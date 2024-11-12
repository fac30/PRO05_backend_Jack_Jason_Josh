namespace J3.Routes;

public static class ColourRoutes
{
  public static void MapColourRoutes(this WebApplication app)
  {
    app.MapGet("/colours", () => {
      return Results.Ok(new[] { "User1", "User2" });
    });

    app.MapGet("/colours/{id}", (int id) => {
      return Results.Ok($"Colour {id}");
    });
  }
}