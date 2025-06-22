using System.ComponentModel.DataAnnotations;

namespace EventManagementApi.DTOs;

public class CreateEventDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    
    [Required]
    public DateTime EventDate { get; set; }
    
    [Required]
    [Range(1, 10000)]
    public int MaxParticipants { get; set; }
} 