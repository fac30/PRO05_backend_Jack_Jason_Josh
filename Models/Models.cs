using System.ComponentModel;

namespace J3.Models;

public class Colour
{
    public int Id { get; set; }
    public required string Hex { get; set; }
}

public class User
{
    public int Id { get; set; }
    public required string requiredName { get; set; }
    public required string Email { get; set; }
    public required string Hash { get; set; }
}

public class Comment
{
    public int Id { get; set; }
    public required string User { get; set; }
    public required string Collection { get; set; }
}

public class Collection
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Name { get; set; }
}

public class ColourCollection
{
    public int Id { get; set; }
    public required string Colour { get; set; }
    public required string Collection { get; set; }
}

public class ColourData
{
    public List<Colour> Rows { get; set; }

    public ColourData()
    {
        Rows = new List<Colour>();
    }
}
