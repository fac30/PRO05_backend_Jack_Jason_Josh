namespace J3.Routes;

public static class UserRoutes
{
  public static void MapUserRoutes(this WebApplication app)
  {
    app.MapGet("/users", () => {
      return Results.Ok(new[] { "Jack", "Jason", "Jack" });
    });

    app.MapGet("/users/{id}", (int id) => {
      return Results.Ok($"User {id}");
    });
  }
}