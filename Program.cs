using J3.Data;
using J3.Models;
using J3.Routes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// Database Context
builder.Services.AddDbContext<ColourContext>(options =>
=======
builder.Services.AddDbContext<ColourContext>(options => 
>>>>>>> main
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IColourContext, ColourContext>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Identity Configuration
builder
    .Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ColourContext>()
    .AddApiEndpoints()
    .AddDefaultTokenProviders(); // Add this line

// Authentication Configuration (Single, Consolidated Setup)
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(
        IdentityConstants.ApplicationScheme,
        options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        }
    );

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        {
<<<<<<< HEAD
            policy.WithOrigins("http://localhost:5174").AllowAnyHeader().AllowAnyMethod();
=======
            policy.WithOrigins("http://localhost:5174")
                .AllowAnyHeader()
                .AllowAnyMethod();
>>>>>>> main
        }
    );
});

builder.Services.AddDataProtection();
builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

<<<<<<< HEAD
app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<User>();

app.MapGet("/", () => "Hello World!");
=======
app.MapGet("/", () => "very front end. much display").WithTags("");
app.MapDevRoutes();
>>>>>>> main
app.MapUserRoutes();
app.MapColourRoutes();
app.MapCollectionRoutes();

app.Run();
