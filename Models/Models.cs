using System.ComponentModel;

namespace J3.Models;

public class Colour
{
    public int Id { get; set; } //todo default, autoincrement
    public required string Hex { get; set; }
}

public class User
{
    public int Id { get; set; } //todo default, autoincrement
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Hash { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
}

public class Collection
{
    public int Id { get; set; } //todo default, autoincrement
    public required string Type { get; set; } //todo default Favourites
    public required bool IsPublic { get; set; } //todo default False
    public required int UserId { get; set; } //todo FK
    public required string Name { get; set; }
    public string Description { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
}

public class ColourCollection
{
    public int Id { get; set; } //todo default, autoincrement
    public required string ColourId { get; set; } //todo FK
    public required string CollectionId { get; set; } //todo FK
    public required int Order { get; set; } //todo Default 0/1
}

public class Comment
{
    public int Id { get; set; } //todo default, autoincrement
    public required int UserId { get; set; } //todo FK
    public required string CollectionId { get; set; }//todo FK
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
