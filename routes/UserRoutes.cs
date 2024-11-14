using J3.Data;
using J3.Models;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes
{
    public static class UserRoutes
    {
        public static void MapUserRoutes(this WebApplication app)
        {
            // GET: /users - Retrieves all users
            app.MapGet(
                "/users",
                async (ColourContext context) =>
                {
                    var users = await context.Users.ToListAsync();
                    return Results.Ok(users);
                }
            );

            // GET: /users/{id} - Retrieves a user by ID
            app.MapGet(
                "/users/{id}",
                async (int id, ColourContext context) =>
                {
                    // Attempts to find the user by its ID in the Users table
                    var user = await context.Users.FindAsync(id);

                    // If the user is not found, return a 404 Not Found response
                    if (user == null)
                    {
                        return Results.NotFound($"User with ID {id} not found.");
                    }

                    // If the user is found, return the user in the response
                    return Results.Ok(user);
                }
            );

            // POST: /users - Creates a new user
            app.MapPost(
                "/users",
                async (User newUser, ColourContext context) =>
                {
                    try
                    {
                        context.Users.Add(newUser);
                        await context.SaveChangesAsync();
                        return Results.Created($"/users/{newUser.Id}", newUser);
                    }
                    catch (Exception ex)
                    {
                        // Log the error if necessary
                        return Results.Problem("An error occurred while creating the user.");
                    }
                }
            );
        }
    }
}
