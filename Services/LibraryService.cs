using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services;

public class LibraryService : ILibraryService
{
    private readonly LibraryDbContext _context;
    
    public LibraryService(LibraryDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserDetailsDto?> GetUserDetailsAsync(int userId)
    {
        var user = await _context.Users
            .Include(u => u.FavoriteBooks)
                .ThenInclude(ubf => ubf.Book)
            .FirstOrDefaultAsync(u => u.Id == userId);
            
        if (user == null)
            return null;
            
        var userDto = new UserDetailsDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            FavoriteBooks = user.FavoriteBooks.Select(ubf => new BookDto
            {
                BookId = ubf.Book.Id,
                Title = ubf.Book.Title,
                Author = ubf.Book.Author
            }).ToList()
        };
        
        return userDto;
    }
    
    public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto updateDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var user = await _context.Users
                .Include(u => u.FavoriteBooks)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (user == null)
                return false;
            
            // Update user data
            user.Name = updateDto.Name;
            user.Email = updateDto.Email;
            
            // Remove all existing favorite books
            _context.UserBookFavorites.RemoveRange(user.FavoriteBooks);
            
            // Add new favorite books
            foreach (var bookId in updateDto.FavoriteBooks)
            {
                // Check if book exists
                var bookExists = await _context.Books.AnyAsync(b => b.Id == bookId);
                if (!bookExists)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                
                var newFavorite = new UserBookFavorite
                {
                    UserId = userId,
                    BookId = bookId
                };
                
                _context.UserBookFavorites.Add(newFavorite);
            }
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
} 