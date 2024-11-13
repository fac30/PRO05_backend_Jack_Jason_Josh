# Entity Framework Core

## Basics

### How does Entity Framework Core Code First work?

1. First Code the model in C# (Classes objects)
2. Describe table keys (with annotations or fluentapi)
3. Create the DbContext
4. Setup the connection with the database
5. Install the packages
6. Then Use EF Migrations to create or update the database
7. Finally, use the DbContext in the code to interact with the database

#### 1. Creating the Models

Each class is its own table in the database.

Good practice: keep the class name singular (e.g. Blog)

```csharp
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public int Rating { get; set; }
    public List<Post> Posts { get; set; }
}
```

#### 2. Creating the DbContext

The DbContext is the client that interacts with the Database

```csharp
public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
}
```

What is a DbSet?

DbSet creates a table from your class Blog

Best practice: plural name your table (e.g. Blogs)

[DBContext Lifetime, Configuration & Initialization](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/?source=post_page-----22b6152bcb81--------------------------------)

#### 3. Set Up the Connection String for testing

Your connection string, for testing (for production use managed identity) you can set it up in your DbContext as:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer(@"Server=DVT-CHANGEMENOW\SQLEXPRESS;Database=CodingWiki;TrustServerCertificate=True;Trusted_Connection=True;");
}
Server= servername
DataBase= databasename
```

#### 4. Migrations

First migration: How to set primary key in EF Core?

For your migrations, the model needs a primary key (PK)

Use the [Key] data annotation to do so (System.ComponentModel.DataAnnotations;)

```csharp
public class Book
{
    [Key] // Sets primary key for IDBook
    public int IDBook { get; set; }

    public string Title { get; set; }
    public string ISBN { get; set; }
    public double Price { get; set; }
}
```

The other way to do it, is simply having a numerical field as “Id” (not case sensitive). Or just having the field ends with Id and starts with class name.

EF Core will automatically set Id as the PK

```csharp
public class Book
{
    public int Id{ get; set; } // automatically sets as PK by EF Core

    public string Title { get; set; }
    public string ISBN { get; set; }
    public double Price { get; set; }
}
```

This also works

```csharp
public class Book
{
    public int BookId{ get; set; } // automatically sets as PK by EF Core

    public string Title { get; set; }
    public string ISBN { get; set; }
    public double Price { get; set; }
}
```

[Code First Conventions](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/conventions/built-in?source=post_page-----22b6152bcb81--------------------------------)

#### 5. Entity Framework Core Packages Explained

```csharp
Microsoft.EntityFrameworkCore.SqlServer: Your main package to create the context and work with EF Core (already includes microsoft.entityframeworkcore package)
Microsoft.EntityFrameworkCore.Tools: gives us the ability to use the dotnet ef command line tool + includes microsoft.entityframeworkcore.design package
Microsoft.EntityFrameworkCore.Design: for the Web project (the starting project)
```

#### 6. Entity Framework Core migration

Once your model and bdContext are built, it’s time to migrate them to the database. For this we use migrations.

Make sure that the default project is where your DbContext is at

![img](https://miro.medium.com/v2/resize:fit:720/format:webp/1*yi1NLJQ6PF44tzrZ9V9i_Q.png)

In the package manager console, write the commands

Create Migration

```zsh
add-migration nameofthemigration
add-migration AddBookToDb
```

Now you have your first migration, see automatically created ‘Migrations’ folder

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*55MnOC1C3gEp2my4zodupA.png)

Apply Migration

After creating a migration, it must be applied in the database

```zsh
update-database
```
For the first migration, it will actually first create the database, then create SQL based on the migration and apply it.

You can now see the database in SSMS

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*b0sxeY8CMWhUcP-f00npnQ.png)

#### 7. Use DbContext in your code

Example add a new product to the database

```csharp
var newProduct = new Product { Name = "Laptop", Price = 999.99 };
dbContext.Products.Add(newProduct); // Add your product in the Db
dbContext.SaveChanges(); // Always save your changes!
```

#### Conclusion

This is the conclusion to the basic guide. You can see how easy it is to work with Entity Framework Core.

I am sure this has also left you with many questions, so the below sections will try to answer those.

Before going through the details, here are some tips

##### Migration

If a migration or an update-database creates an error, ALWAYS first remove the migration and then create a new one with the corrections made in the code.

Make small changes and small migrations

Always check the migration file before updating database

##### Annotations vs fluent api

which one to choose?
Use annotations for small projects and fluent api for larger ones

---

### Entity Framework Core Migrations

#### When to use migration?

- New class/ table added to DB
- New property/ column
- Existing property/ column modified
- Property/ column deleted
- Class/ table deleted

#### Update the database

How to update the database in EF Core code first?

Simply create a new migration

add-migration yourFavouriteMigrationName
You can see the results in the migation file

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*nwdPDAzWAkzbN881jIcx4g.png)

Please note that if you don’t run update-database, the database won’t be well…updated

#### Remove migration

Only use it when the migration hasn’t been pushed the database and you want to revert it

Warning: Use this command only if the migration hasn’t been applied to the database!!!

remove-migration
Do not remove any migration files in the Migrations folder or change any files directly

#### How to revert back to an old migration?

Copy the name of the migration you previously applied

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*r9sSDRtGr0qIiajqagud6w.png)

Here the name is: “AddBookToDb”

Append the name to update-database

update-database AddBookToDb
See reverting results

![img](https://miro.medium.com/v2/resize:fit:720/format:webp/1*pUNem4iA6w3Jjkrn-8giNg.png)

To come back to the latest version, simply do

update-database
It will reapply all the migrations that have been reverted

#### How to revert a migration in Entity Franework Core?

Do as previous step: update-data nameofmigrationyouwanttogobackto

update-database nameofmigrationyouwanttogobackto
do the changes you wish to make, make a migration

add-migration undidChanges

#### How to see what migration has been applied in EntityFramework Core?

In PMC just run:

get-migration
The resulting table will indicate which migration has been applied or not

![img](https://miro.medium.com/v2/resize:fit:720/format:webp/1*ZbEy8tFQVfeG0m7bthYJrA.png)

#### How to delete the database in EntityFramework Core?

In PMC just run:

drop-database
To get the database back, just run

update-database
All migrations will be re-applied

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*q5sDV6yeoD_w8k4C6Xn7yQ.png)

### Entity Framework Core Data Annotations

EF Core lets you work the SQL table directly in code using only annotations!

![img](https://miro.medium.com/v2/resize:fit:720/format:webp/0*YOkMiJF59CImPDLO.png)

#### How to change table or column name in EntityFramework Core?

Tavle names are set in DbContext:

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*6I36-N2SDEAp3pzj9SR4Fw.png)

However, we can change this using data annotation:

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*KUpIrKfyMV4J5ufKokz_Rg.png)

Then we finish by applying a new migration

#### How to set a field as required in EntityFramework Core?

By clicking on your project, you can see if nullable is enabled

![img](https://miro.medium.com/v2/resize:fit:720/format:webp/1*KyPl2yeFq_e8VbhZBBtpbQ.png)

Because of that, all string properties are required by default, to override this, we need to ask the nullable “?”, e.g.:

        public string? GenreName { get; set; }
Without the <Nullable>enable</Nullable> the properties will be nullable by default

In case you don’t have the nullable enabled and you want a property to be required, you can use:

[Required (ErrorMessage = "Genre name is required")]
public string GenreName { get; set; }

#### How to ensure a property doesn’t get in the table in EntityFramework Core?

Sometimes we just want the field to be available in the code but not as a column, for such case use:

[NotMapped]
public string PriceRange { get; set; }

#### How to create foreign keys in EntityFramework Core with Data annotations?

##### 1:1 Relationship

In a 1:1 relationship there is a child (dependent class) and parent (main class)

A foreign key, has to be placed in the child class and a reference to the child class must be present in the parent class

public class Book
{
    public int BookId { get; set; } // the PK
    public BookDetail BookDetail { get; set; } //the child class reference
}

public class BookDetail
{
    [Key]
    public int BookDetail_Id { get; set; }

    [ForeignKey("Book")] // "Book" indicates  the class the FK applies to
    public int BookId { get; set; } // the FK
   
    public Book Book { get; set; } // the reference to the parent class
See resulting migation

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*7VtWyTdbrly_BRnGr_q1yQ.png)

###### How to make the primary key custom generated in EntityFramework Core Data Annotations?

Use the DatabaseGenerated annotation

[DatabaseGenerated(DatabaseGeneratedOption.None)]: no DB generated IDs
[DatabaseGenerated(DatabaseGeneratedOption.Computed)]: DB generated
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.None)] // the DB won't create the Id
public int DealerID { get; set; }

[DatabaseGenerated Attribute](https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes/databasegenerated-attribute?source=post_page-----22b6152bcb81--------------------------------)

##### 1:Many Relationship

In our example we want books to have a 1:many relationship to publishers

i.e. a publisher can have many books, a book can only have one publisher

public class Book
{
    // [Key]
    public int BookId { get; set; }
    
    [ForeignKey("Publisher")]
    public int Publisher_Id { get; set; }
    public Publisher Publisher { get; set; }
}

public class Publisher
{
    [Key]
    public int Publisher_Id { get; set; }

    public List<Book> Books { get; set; } // add FK object as List
}

##### Many:Many Relationship

Auhtors can have many books and books can have multiple authors

public class Author
{
    [Key] 
    public int Author_Id { get; set; }

    public List<Book> Books { get; set; } // Adds relationship to Book

}
We simply add

public class Book
{
    // [Key]
    public int BookId { get; set; }

    public List<Author> Authors { get; set; } // Adds relationship to Authors
}
EF Core (version 7+) automatically creates an intermediary table (mapping table) beteen Book and Author

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*H3tWRgdOPO8Ajx_u_Gx__w.png)

###### How to manually create the intermediate mapping table in EntityFramework Core?

Create the following table with explicit FK for many:many relationship between author and book

public class BookAuthorMap
{
    [ForeignKey("Book")]
    public int Book_Id { get; set; }

    [ForeignKey("Author")]
    public int Author_Id { get; set; }

    public Book Book { get; set; }
    public Author Author { get; set; }
}
Then add mapping to respective classes, adding BookAuthorMap

public class Author
{
    [Key] 
    public int Author_Id { get; set; }
    
    public List<BookAuthorMap> BookAuthorMap { get; set; }

}

 public class Book
 {
     // [Key]
     public int BookId { get; set; }

     public List<BookAuthorMap> BookAuthorMap{ get; set; }
 }
Nevertheless, the table BookAuthorMap needs a composite key, this can be done using Fluent API

###### How to add composite key in EntityFramework Core Data Annotations?

Since EF Core 7+, data annotation is available for composite key

[PrimaryKey("ProductID", "ModelID")]
internal class ProductModel
{
     public int ProductID { get; set; }
     public int ModelID { get; set; }
}

[Keys in EF Core](https://learn.microsoft.com/en-us/ef/core/modeling/keys?tabs=data-annotations&source=post_page-----22b6152bcb81--------------------------------)

Resulting migration with two PKs


And resulting diagram

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*pnSNXoCcOPxHfRRXAAMDDA.png)

###### How to create many:many relationship with skip mapping table in EntityFramework Core Data Annotations?

Books can have multiple authors
Authors can have multiple books
No fluent api is even required

public class Fluent_Book
 {
     [Key]
     public int BookId { get; set;}
     public List<Fluent_Author>Authors{ get; set; }
 }

public class Fluent_Author
 {
     [Key] 
     public int Author_Id { get; set; }
     public List<Fluent_Book> Books { get; set; }
 }
 Resulting mapping table (Fluent_AuthorFluent_Book)

![img](https://miro.medium.com/v2/resize:fit:640/format:webp/1*YR71P1yZtlvtquXIdwFVsg.png)

## Fluent API in EntityFramework Core

![img](https://miro.medium.com/v2/resize:fit:720/format:webp/0*8IG407_FeKIk7fjs.png)

Introduction
Data annotations are great but they are also clustered all over your classes, perhaps you would want to have it all one place?

Also, fluent API can add many customisation that Data annotations cannot do,

To use it, simply add code in the OnModelCreating

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Fluent API code comes here
}
How to change table name with EntityFramework Core Fluent API?
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder.Entity<Category>().ToTable("tb_Category");
}
How to change column name with EntityFramework Core Fluent API?
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder.Entity<Category>()
              .Property(c => c.ISBN)
              .HasColumnName("ISBN");
}
How to make a column required with EntityFramework Core Fluent API?
modelBuilder.Entity<Category>()
          .Property(c => c.Title)
          .IsRequired();
How to add a primary key with EntityFramework Core Fluent API?
modelBuilder.Entity<Category>()
          .HasKey(c => c.CategoryId);
How to set Max Length with EntityFramework Core Fluent API?
modelBuilder.Entity<Category>()
          .Property(c => c.CategoryName)
          .HasMaxLength(50);
How to set Not Mapped with EntityFramework Core Fluent API?
modelBuilder.Entity<Category>()
          .Ignore(c => c.DiscountedPrice);
How to set 1:1 relationship with EntityFramework Core Fluent API?
Book detail can only have one book (HasOne)
And book only have one book detail (WithOne)
The connection (ForeignKey) is BookId
modelBuilder.Entity<Fluent_BookDetail>()
          .HasOne(b => b.Book) // Object in Fluent_BookDetail
          .WithOne(bd => bd.BookDetail) // Object in Fluent_Book
          .HasForeignKey<Fluent_BookDetail>(b => b.BookId); 
          // Foreign Key can be in either Fluent_BookDetail OR Fluent_Book
public class Fluent_BookDetail
{
  [Key]
  public int BookDetail_Id { get; set; }

  public int BookId { get; set; }

  public Fluent_Book Book { get; set; }
}

public class Fluent_Book
{
  [Key]
  public int BookId { get; set; }

  public Fluent_BookDetail BookDetail { get; set; }

}
How to set 1:many relationship with EntityFramework Core Fluent API?
One publisher can have many books
One book only has one publisher
modelBuilder.Entity<Fluent_Book>() 
          .HasOne(u => u.Publisher) 
          .WithMany(u => u.Books)
          .HasForeignKey(u => u.Publisher_Id); // FK in Fluent_Book
// FK always in entity that has the 1 side of the 1-many relationship
public class Fluent_Book
{
  [Key]
  public int BookId { get; set; }

  public int Publisher_Id { get; set; }
  public Fluent_Publisher Publisher { get; set; }
}

public class Fluent_Publisher
{
  [Key]
  public int Publisher_Id { get; set; }

  public List<Fluent_Book> Books { get; set; }
}
Composite Keys in Entity Framework Core Fluent API
Composite keys allow two PK to be combined and form a unique PK

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder.Entity<BookAuthorMap>()
  .HasKey(ba => new 
  { ba.Book_Id, 
    ba.Author_Id 
  });
How to manually create many:many relationship with fluent API in EntityFramework Core?
Books can have multiple authors
Authors can have multiple books
Create mapping table
MappingTables don’t need their own DbSet<MappingTable>

// Mapping Table
public class Fluent_BookAuthorMap
{
  [Key]
  public int Book_Id { get; set; }
  public int Author_Id { get; set; }

  public Fluent_Book Book { get; set; }
  public Fluent_Author Author { get; set; }
}

public class Fluent_Book
{
  [Key]
  public int BookId { get; set; }
  public List<Fluent_BookAuthorMap> BookAuthorMap { get; set; }
}

public class Fluent_Author
{
  [Key] 
  public int Author_Id { get; set; }

  public List<Fluent_BookAuthorMap> BookAuthorMap { get; set; }
}
In DbContext:

Create Composite key
1:many fluent_book to fluent_book_author
1:many fluent_book_author to fluent_book
// composite key
modelBuilder.Entity<Fluent_BookAuthorMap>()
          .HasKey(ba => new
          {
              ba.Book_Id,
              ba.Author_Id
          });
// 1:many between fluent_book & fluent_book_author
modelBuilder.Entity<Fluent_BookAuthorMap>()
          .HasOne(b => b.Book)
          .WithMany(ba => ba.BookAuthorMap)
          .HasForeignKey(b => b.Book_Id);

// 1:many between fluent_author & fluent_book
modelBuilder.Entity<Fluent_BookAuthorMap>()
          .HasOne(a => a.Author)
          .WithMany(ba => ba.BookAuthorMap)
          .HasForeignKey(a => a.Author_Id);
How to set Cascade options in Fluent API EntityFramework Core?
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder
      .Entity<Blog>()
      .HasOne(e => e.Owner)
      .WithOne(e => e.OwnedBlog)
      .OnDelete(DeleteBehavior.ClientCascade);
}

[Casecade Delete](https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete?source=post_page-----22b6152bcb81--------------------------------)

What does onDelete: ReferencetialAction.Cascade mean in EntityFramework Core?

> For rest of article, [click here](https://medium.com/@codebob75/entity-framework-core-code-first-introduction-best-practices-repository-pattern-clean-22b6152bcb81)
