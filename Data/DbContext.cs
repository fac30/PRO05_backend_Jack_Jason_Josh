using Microsoft.EntityFrameworkCore;
using J3.Models;

namespace J3.Data
{
    public class ColourContext : DbContext
    {
        public ColourContext(DbContextOptions<ColourContext> options)
            : base(options)
        {
        } //todo What's this?

        public DbSet<Colour> Colours { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<ColourCollection> ColourCollections { get; set; }
        public DbSet<Comment> Comments { get; set; }       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Collection>().ToTable("collection");
            modelBuilder.Entity<ColourCollection>().ToTable("colour_collection");
            modelBuilder.Entity<Colour>().ToTable("colour");
            modelBuilder.Entity<Comment>().ToTable("comment");
            modelBuilder.Entity<User>().ToTable("user");

            modelBuilder
                .Entity<ColourCollection>()
                .HasKey(cc => new { cc.CollectionId, cc.ColourId });
        }
    }
}

/* Fluent API
  Notes for when I implement this later
  
  public class ApplicationDbContext : DbContext
  {
      // ... other context code ...
  
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<User>()
              .Property(u => u.CreatedAt)
              .HasDefaultValueSql("CURRENT_TIMESTAMP");
              
          // ... other configurations ...
      }
  } */