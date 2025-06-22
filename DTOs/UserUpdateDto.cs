using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class UserUpdateDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    public List<int> FavoriteBooks { get; set; } = new();
} 