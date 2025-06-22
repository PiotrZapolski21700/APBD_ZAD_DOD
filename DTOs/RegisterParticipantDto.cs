using System.ComponentModel.DataAnnotations;

namespace EventManagementApi.DTOs;

public class RegisterParticipantDto
{
    [Required]
    public int EventId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = null!;
    
    [Phone]
    [MaxLength(20)]
    public string? Phone { get; set; }
} 