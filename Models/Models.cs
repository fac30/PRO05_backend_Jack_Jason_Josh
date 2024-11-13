using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
/* Data Annotations Schema
    - This is required in order to use Data Annotations
    - You can see one in User.CreatedAt
    - [DatabaseGenerated()] is a Data Annotation
*/

namespace J3.Models;

/* How This File Relates to DbContext
    - The database is created by combining this file with DbContext
        - This file *defines* the...
            - Name of a single entry in a Table (as Classes)
            - Columns (as Properties within those Classes)
        - DbContext will then *build* those table like so:
            public DbSet<Colour> Colours { get; set; }
            - Build a table • DbSet
            - The table has these columns • <Colour>
            - The table has this name • Colours
    - EF Core automatically sets any numerical field called "Id" as the Primary Key
*/

public class Colour
{
    public int Id { get; set; }
    public required string Hex { get; set; }
}

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Hash { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
}

public class Collection //todo x • •
{
    public int Id { get; set; }
    public required string Type { get; set; } //todo def ""
    public required bool IsPublic { get; set; } //todo def false
    public required int UserId { get; set; }
    public User User { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
}

public class ColourCollection //todo x x •
{
    public int Id { get; set; }
    public required int ColourId { get; set; }
    public Colour Colour { get; set; }
    public required int CollectionId { get; set; }
    public Collection Collection { get; set; }
    public required int Order { get; set; } //todo Default 0/1
}

public class Comment //todo x x •
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public User User { get; set; }
    public required int CollectionId { get; set; }
    public Collection Collection { get; set; }
    public required int Content { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //todo Data Annotation
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
