namespace EventManagementApi.DTOs;

public class ParticipantReportDto
{
    public int ParticipantId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<ParticipantEventDto> Events { get; set; } = new();
}

public class ParticipantEventDto
{
    public int EventId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancellationDate { get; set; }
    public List<string> Speakers { get; set; } = new();
} 