using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace J3.Models;

public class Colour
{
    public int Id { get; set; }
    public required string Hex { get; set; }
    public string? colourName { get; set; }
}

public class User : IdentityUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Collection
{
    public int Id { get; set; }
    public required string Type { get; set; } = "";
    public required bool IsPublic { get; set; } = false;
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string UserId { get; set; }
    public User? User { get; set; }
    public ICollection<ColourCollection>? ColourCollections { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ColourCollection
{
    public int Id { get; set; }
    public required int CollectionId { get; set; }
    public Collection? Collection { get; set; }
    public required int ColourId { get; set; }
    public Colour? Colour { get; set; }
    public required int Order { get; set; } = 0;
}

public class Comment
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public User? User { get; set; }
    public required int CollectionId { get; set; }
    public Collection? Collection { get; set; }
    public required int Content { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ColourData // JSON Utility Class
{
    public List<Colour> Rows { get; set; }

    public ColourData()
    {
        Rows = new List<Colour>();
    }
}
