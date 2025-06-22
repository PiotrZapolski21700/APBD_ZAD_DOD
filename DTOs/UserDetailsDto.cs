namespace LibraryApi.DTOs;

public class UserDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<BookDto> FavoriteBooks { get; set; } = new();
} 