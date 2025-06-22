using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Models;

public class User
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(150)]
    [Column("Name")]
    public string Name { get; set; } = null!;
    
    [Required]
    [MaxLength(150)]
    [Column("Email")]
    public string Email { get; set; } = null!;
    
    public virtual ICollection<UserBookFavorite> FavoriteBooks { get; set; } = new List<UserBookFavorite>();
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
} 