using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementApi.Models;

[Table("Participants")]
public class Participant
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column("FirstName")]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    [Column("LastName")]
    public string LastName { get; set; } = null!;
    
    [Required]
    [MaxLength(150)]
    [Column("Email")]
    public string Email { get; set; } = null!;
    
    [MaxLength(20)]
    [Column("Phone")]
    public string? Phone { get; set; }
    
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }
    
    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
} 