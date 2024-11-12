using System.ComponentModel;

namespace j3.models;

class Colour
{
    public int Id { get; set; }
    public required string Hex { get; set; }
}

class User
{
    public int Id { get; set; }
    public required string requiredName { get; set; }
    public required string Email { get; set; }
    public required string Hash { get; set; }
}

class Comment
{
    public int Id { get; set; }
    public required string User { get; set; }
    public required string Collection { get; set; }
}

class Collection
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Name { get; set; }
}

class ColourCollection
{
    public int Id { get; set; }
    public required string Colour { get; set; }
    public required string Collection { get; set; }
}
