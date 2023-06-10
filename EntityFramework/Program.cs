using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EF;
public class userdb
{
    public int id { get; set; }
    public string name { get; set; }
    public int age { get; set; }
}

public class LibraryContext : DbContext
{
    public DbSet<userdb> userdb { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=localhost;database=users;user=root;password=parashaebanaya");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<userdb>(entity =>
        {
            entity.HasKey(e => e.name);
            entity.Property(e => e.age).IsRequired();
        });
    }
}
class Program
{
    static void Main(string[] args)
    {
        InsertData();
        PrintData();
    }

    private static void InsertData()
    {
        using (var context = new LibraryContext())
        {
            // Creates the database if not exists
            context.Database.EnsureCreated();
            // Adds some books
            context.userdb.Add(new userdb
            {
                name = "978-0544003415",
                age = 12
            });
            context.userdb.Add(new userdb
            {
                name = "978-0547247762",
                age = 41
            });

            // Saves changes
            context.SaveChanges();
        }
    }

    private static void PrintData()
    {
        // Gets and prints all books in database
        using (var context = new LibraryContext())
        {
            var books = context.userdb;
            foreach (var book in books)
            {
                var data = new StringBuilder();
                data.AppendLine($"ISBN: {book.name}");
                data.AppendLine($"Title: {book.age}");
                Console.WriteLine(data.ToString());
            }
        }
    }
}