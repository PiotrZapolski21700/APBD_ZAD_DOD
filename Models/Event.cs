using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementApi.Models;

[Table("Events")]
public class Event
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    [Column("Title")]
    public string Title { get; set; } = null!;
    
    [Required]
    [MaxLength(1000)]
    [Column("Description")]
    public string Description { get; set; } = null!;
    
    [Required]
    [Column("EventDate")]
    public DateTime EventDate { get; set; }
    
    [Required]
    [Column("MaxParticipants")]
    public int MaxParticipants { get; set; }
    
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }
    
    [Column("UpdatedAt")]
    public DateTime UpdatedAt { get; set; }
    
    public virtual ICollection<EventSpeaker> EventSpeakers { get; set; } = new List<EventSpeaker>();
    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
} 