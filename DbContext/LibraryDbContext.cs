using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data;

public class LibraryDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<UserBookFavorite> UserBookFavorites { get; set; }
    public DbSet<Loan> Loans { get; set; }
    
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure composite keys
        modelBuilder.Entity<UserBookFavorite>()
            .HasKey(ubf => new { ubf.BookId, ubf.UserId });
            
        modelBuilder.Entity<Loan>()
            .HasKey(l => new { l.BookId, l.UserId });
        modelBuilder.Entity<Loan>()
            .Property(l => l.UserId)
            .HasColumnName("ID");
        modelBuilder.Entity<Loan>()
            .Property(l => l.BookId)
            .HasColumnName("Book_ID");
        
        // Configure relationships
        modelBuilder.Entity<UserBookFavorite>()
            .HasOne(ubf => ubf.User)
            .WithMany(u => u.FavoriteBooks)
            .HasForeignKey(ubf => ubf.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<UserBookFavorite>()
            .HasOne(ubf => ubf.Book)
            .WithMany(b => b.UserFavorites)
            .HasForeignKey(ubf => ubf.BookId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.User)
            .WithMany(u => u.Loans)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Anna Nowak", Email = "anna.nowak@example.com" },
            new User { Id = 2, Name = "Jan Kowalski", Email = "jan.kowalski@example.com" },
            new User { Id = 3, Name = "Maria Wiśniewska", Email = "maria.wisniewska@example.com" }
        );
        
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 101, Title = "Wiedźmin", Author = "Andrzej Sapkowski" },
            new Book { Id = 102, Title = "Solaris", Author = "Stanisław Lem" },
            new Book { Id = 103, Title = "Pan Tadeusz", Author = "Adam Mickiewicz" },
            new Book { Id = 104, Title = "Lalka", Author = "Bolesław Prus" },
            new Book { Id = 105, Title = "Mały Książę", Author = "Antoine de Saint-Exupéry" },
            new Book { Id = 201, Title = "Harry Potter i Kamień Filozoficzny", Author = "J.K. Rowling" },
            new Book { Id = 205, Title = "Władca Pierścieni", Author = "J.R.R. Tolkien" },
            new Book { Id = 210, Title = "1984", Author = "George Orwell" }
        );
        
        modelBuilder.Entity<UserBookFavorite>().HasData(
            new UserBookFavorite { UserId = 1, BookId = 101 },
            new UserBookFavorite { UserId = 1, BookId = 105 },
            new UserBookFavorite { UserId = 2, BookId = 102 },
            new UserBookFavorite { UserId = 2, BookId = 103 }
        );
        
        modelBuilder.Entity<Loan>().HasData(
            new Loan { UserId = 1, BookId = 101, LoanDate = new DateTime(2024, 1, 15), ReturnDate = new DateTime(2024, 2, 1) },
            new Loan { UserId = 1, BookId = 105, LoanDate = new DateTime(2024, 3, 1), ReturnDate = null },
            new Loan { UserId = 2, BookId = 102, LoanDate = new DateTime(2024, 2, 10), ReturnDate = new DateTime(2024, 3, 5) }
        );
    }
} 