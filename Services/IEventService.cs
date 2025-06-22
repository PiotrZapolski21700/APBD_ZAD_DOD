using EventManagementApi.DTOs;

namespace EventManagementApi.Services;

public interface IEventService
{
    Task<ServiceResult<int>> CreateEventAsync(CreateEventDto dto);
    Task<ServiceResult<bool>> AssignSpeakerAsync(AssignSpeakerDto dto);
    Task<ServiceResult<List<EventDetailsDto>>> GetUpcomingEventsAsync();
} 