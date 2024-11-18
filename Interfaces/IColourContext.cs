using J3.Models;
using Microsoft.EntityFrameworkCore;

namespace J3.Data
{
    public interface IColourContext
    {
        DbSet<Colour> Colours { get; }
        DbSet<User> Users { get; }
        DbSet<Collection> Collections { get; }
        DbSet<ColourCollection> ColourCollections { get; }
        DbSet<Comment> Comments { get; }
        int SaveChanges();
    }
}
