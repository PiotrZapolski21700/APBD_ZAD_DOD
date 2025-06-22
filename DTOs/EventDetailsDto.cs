namespace EventManagementApi.DTOs;

public class EventDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public int MaxParticipants { get; set; }
    public int RegisteredParticipants { get; set; }
    public int AvailableSeats { get; set; }
    public List<SpeakerDto> Speakers { get; set; } = new();
}

public class SpeakerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
} 