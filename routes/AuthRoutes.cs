namespace J3.Routes;

using System.Security.Claims;
using J3.Models;
using Microsoft.AspNetCore.Identity;

public static class AuthRoutes
{
    public static void MapAuthRoutes(this WebApplication app)
    {
        app.MapGet(
                "/auth/test",
                (HttpContext context) =>
                {
                    if (context.User.Identity?.IsAuthenticated ?? false)
                    {
                        return Results.Ok(
                            new { Message = "Authenticated", User = context.User.Identity.Name }
                        );
                    }
                    return Results.Unauthorized();
                }
            )
            .RequireAuthorization()
            .WithTags("Authentication");

        app.MapGet(
                "/auth/status",
                (HttpContext context) =>
                {
                    var identity = context.User.Identity;
                    var currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    return Results.Ok(
                        new
                        {
                            IsAuthenticated = identity?.IsAuthenticated ?? false,
                            Username = identity?.Name,
                            AuthType = identity?.AuthenticationType,
                            id = currentUserId,
                            Claims = context
                                .User.Claims.Select(c => new { c.Type, c.Value })
                                .ToList(),
                        }
                    );
                }
            )
            .WithTags("Authentication");

        app.MapPost(
                "/auth/logout",
                async (HttpContext context, SignInManager<User> signInManager) =>
                {
                    try
                    {
                        await signInManager.SignOutAsync();
                        return Results.Ok(new { message = "Successfully logged out" });
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new { message = $"Logout failed: {ex.Message}" });
                    }
                }
            )
            .WithTags("Authentication")
            .RequireAuthorization();
    }
}
