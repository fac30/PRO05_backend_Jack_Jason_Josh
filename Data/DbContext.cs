using J3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace J3.Data;

public class ColourContext : IdentityDbContext<User>, IColourContext
{
    public ColourContext(DbContextOptions<ColourContext> options)
        : base(options) { }

    public DbSet<Colour> Colours { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<ColourCollection> ColourCollections { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Comment>().ToTable("comment");
        modelBuilder.Entity<Collection>().ToTable("collection");
        modelBuilder.Entity<ColourCollection>().ToTable("colour_collection");
        modelBuilder.Entity<Colour>().ToTable("colour");
        modelBuilder.Entity<User>().ToTable("user");

        modelBuilder.Entity<ColourCollection>().HasKey(cc => new { cc.CollectionId, cc.ColourId });
    }
}
