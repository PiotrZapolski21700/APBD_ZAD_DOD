using System.ComponentModel.DataAnnotations;

namespace EventManagementApi.DTOs;

public class AssignSpeakerDto
{
    [Required]
    public int EventId { get; set; }
    
    [Required]
    public int SpeakerId { get; set; }
} 