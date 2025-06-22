namespace LibraryApi.DTOs;

public class BookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
} 