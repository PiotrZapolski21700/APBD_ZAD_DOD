using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementApi.Models;

[Table("EventParticipants")]
public class EventParticipant
{
    [Key, Column("EventId", Order = 0)]
    public int EventId { get; set; }
    
    [Key, Column("ParticipantId", Order = 1)]
    public int ParticipantId { get; set; }
    
    [Column("RegistrationDate")]
    public DateTime RegistrationDate { get; set; }
    
    [Column("IsCancelled")]
    public bool IsCancelled { get; set; }
    
    [Column("CancellationDate")]
    public DateTime? CancellationDate { get; set; }
    
    [ForeignKey(nameof(EventId))]
    public virtual Event Event { get; set; } = null!;
    
    [ForeignKey(nameof(ParticipantId))]
    public virtual Participant Participant { get; set; } = null!;
} 