using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Models;

public class UserBookFavorite
{
    [Key, Column("Book_ID", Order = 0)]
    public int BookId { get; set; }
    
    [Key, Column("User_ID", Order = 1)]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(BookId))]
    public virtual Book Book { get; set; } = null!;
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
} 