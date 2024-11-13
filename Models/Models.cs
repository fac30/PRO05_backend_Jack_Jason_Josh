using System.ComponentModel;

namespace J3.Models;

// todo Id: default & autoincrement
public class Colour
{
    public int Id { get; set; }
    public required string Hex { get; set; }
}

// todo Id: default & autoincrement
public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Hash { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
}

// todo Id: default & autoincrement
// todo Type: default Favourites
// todo IsPublic: default False
// todo UserId: FK
public class Collection
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public required bool IsPublic { get; set; }
    public required int UserId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
}

// todo Id: default & autoincrement
// todo ColourId: FK Colour
// todo CollectionId: FK Collection
// todo Order: Default 0/1
public class ColourCollection
{
    public int Id { get; set; }
    public required string ColourId { get; set; }
    public required string CollectionId { get; set; }
    public required int Order { get; set; }
}

// todo Id: default & autoincrement
// todo UserId: FK User
// todo Collection Id: FK Collection
public class Comment
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required string CollectionId { get; set; }
    public required string Content { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
}

/* JSON Utility Classes
  public class ColourData
  {
      public List<Colour> Rows { get; set; }

      public ColourData()
      {
          Rows = new List<Colour>();
      }
  } */
