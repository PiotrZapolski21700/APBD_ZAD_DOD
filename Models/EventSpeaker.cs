using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementApi.Models;

[Table("EventSpeakers")]
public class EventSpeaker
{
    [Key, Column("EventId", Order = 0)]
    public int EventId { get; set; }
    
    [Key, Column("SpeakerId", Order = 1)]
    public int SpeakerId { get; set; }
    
    [Column("AssignedAt")]
    public DateTime AssignedAt { get; set; }
    
    [ForeignKey(nameof(EventId))]
    public virtual Event Event { get; set; } = null!;
    
    [ForeignKey(nameof(SpeakerId))]
    public virtual Speaker Speaker { get; set; } = null!;
} 