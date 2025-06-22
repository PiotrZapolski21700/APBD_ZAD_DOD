using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Models;

public class Book
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(150)]
    [Column("Title")]
    public string Title { get; set; } = null!;
    
    [Required]
    [MaxLength(150)]
    [Column("Author")]
    public string Author { get; set; } = null!;
    
    public virtual ICollection<UserBookFavorite> UserFavorites { get; set; } = new List<UserBookFavorite>();
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
} 