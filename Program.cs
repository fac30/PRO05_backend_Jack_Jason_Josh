using J3.Data;
using J3.Routes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* Default Connection
  This line specifies where we pull the location, username & password of the PostgreSQL database from. */
builder.Services.AddDbContext<ColourContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register the IColourContext interface to ColourContext implementation
builder.Services.AddScoped<IColourContext, ColourContext>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5174") // Allow your frontend origin
                .AllowAnyHeader() // Allow any headers
                .AllowAnyMethod(); // Allow any HTTP method (GET, POST, etc.)
        }
    );
});

var app = builder.Build();

// Enable CORS middleware
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");
app.MapUserRoutes();
app.MapColourRoutes();

app.Run();
