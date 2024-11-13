using Microsoft.EntityFrameworkCore;
using J3.Routes;

var builder = WebApplication.CreateBuilder(args);

/* Default Connection
  This line specifies where we pull the location, username & password of the PostgreSQL database from. */
builder.Services.AddDbContext<ColourContext>(
  options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")
  )
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");
app.MapUserRoutes();
app.MapColourRoutes();

app.Run();
