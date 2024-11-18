using J3.Models;
<<<<<<< HEAD
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
=======
>>>>>>> main
using Microsoft.EntityFrameworkCore;

namespace J3.Data;


public class ColourContext : DbContext, IColourContext
{
<<<<<<< HEAD
    public class ColourContext : IdentityDbContext<User>, IColourContext
=======
    public ColourContext(DbContextOptions<ColourContext> options)
        : base(options) { }

    public DbSet<Colour> Colours { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<ColourCollection> ColourCollections { get; set; }
    public DbSet<Comment> Comments { get; set; }       

    protected override void OnModelCreating(ModelBuilder modelBuilder)
>>>>>>> main
    {
        base.OnModelCreating(modelBuilder);

<<<<<<< HEAD
        public DbSet<Colour> Colours { get; set; }

        // public DbSet<User> Users { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<ColourCollection> ColourCollections { get; set; }
        public DbSet<Comment> Comments { get; set; }
=======
        modelBuilder.Entity<Colour>()
            .ToTable("colour");
        
        modelBuilder.Entity<User>()
            .ToTable("user");
        
        modelBuilder.Entity<Collection>()
            .ToTable("collection");
        
        modelBuilder.Entity<ColourCollection>()
            .ToTable("colour_collection");
        modelBuilder.Entity<ColourCollection>()
            .HasKey(cc => new { cc.CollectionId, cc.ColourId });
>>>>>>> main

        modelBuilder.Entity<Comment>()
            .ToTable("comment");

<<<<<<< HEAD
            modelBuilder.Entity<Collection>().ToTable("collection");
            modelBuilder.Entity<ColourCollection>().ToTable("colour_collection");
            modelBuilder.Entity<Colour>().ToTable("colour");
            modelBuilder.Entity<Comment>().ToTable("comment");
            modelBuilder.Entity<User>().ToTable("user");

            // modelBuilder.HasDefaultSchema("identity");

            modelBuilder
                .Entity<ColourCollection>()
                .HasKey(cc => new { cc.CollectionId, cc.ColourId });
        }
=======
>>>>>>> main
    }
}
