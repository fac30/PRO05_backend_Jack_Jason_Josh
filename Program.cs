using J3.ColourExtensions;
using J3.Data;
using J3.Models;
using J3.Routes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database Context
builder.Services.AddDbContext<ColourContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IColourContext, ColourContext>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Identity Configuration (this includes authentication)
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<ColourContext>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:6969")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

// Other services
builder.Services.AddDataProtection();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddAuthorization();

builder.Services.AddHttpClient<ColourNameExtensions>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

// Use CORS before authentication
app.UseCors("AllowFrontend");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthRoutes();
app.MapColourRoutes();
app.MapCollectionRoutes();
app.MapDevRoutes();
app.MapUserRoutes();
app.MapUtilRoutes();

app.MapIdentityApi<User>()
   .WithTags("Authentication");

app.MapGet("/", () => "JJJ API is running!").WithTags("");

app.Run();
