using EventManagementApi.Data;
using EventManagementApi.DTOs;
using EventManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApi.Services;

public class EventService : IEventService
{
    private readonly EventDbContext _context;
    
    public EventService(EventDbContext context)
    {
        _context = context;
    }
    
    public async Task<ServiceResult<int>> CreateEventAsync(CreateEventDto dto)
    {
        if (dto.EventDate <= DateTime.UtcNow)
        {
            return new ServiceResult<int>
            {
                Success = false,
                Message = "Event date must be in the future",
                ErrorType = ErrorType.ValidationError
            };
        }
        var newEvent = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            EventDate = dto.EventDate,
            MaxParticipants = dto.MaxParticipants,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        try
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return new ServiceResult<int>
            {
                Success = true,
                Message = "Event created successfully",
                Data = newEvent.Id
            };
        }
        catch (Exception)
        {
            return new ServiceResult<int>
            {
                Success = false,
                Message = "Database error occurred while creating event",
                ErrorType = ErrorType.DatabaseError
            };
        }
    }
    public async Task<ServiceResult<bool>> AssignSpeakerAsync(AssignSpeakerDto dto)
    {
        var eventExists = await _context.Events.AnyAsync(e => e.Id == dto.EventId);
        if (!eventExists)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = $"Event with ID {dto.EventId} not found",
                ErrorType = ErrorType.NotFound
            };
        }
        var speaker = await _context.Speakers
            .Include(s => s.EventSpeakers)
                .ThenInclude(es => es.Event)
            .FirstOrDefaultAsync(s => s.Id == dto.SpeakerId);
        if (speaker == null)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = $"Speaker with ID {dto.SpeakerId} not found",
                ErrorType = ErrorType.NotFound
            };
        }
        var targetEvent = await _context.Events.FindAsync(dto.EventId);
        var alreadyAssigned = await _context.EventSpeakers
            .AnyAsync(es => es.EventId == dto.EventId && es.SpeakerId == dto.SpeakerId);
        if (alreadyAssigned)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Speaker is already assigned to this event",
                ErrorType = ErrorType.Conflict
            };
        }
        var hasTimeConflict = speaker.EventSpeakers
            .Any(es => !es.Event.Id.Equals(dto.EventId) && 
                      es.Event.EventDate.Date == targetEvent!.EventDate.Date &&
                      Math.Abs((es.Event.EventDate - targetEvent.EventDate).TotalHours) < 4);
        if (hasTimeConflict)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Speaker has a time conflict with another event",
                ErrorType = ErrorType.BusinessRuleViolation
            };
        }
        try
        {
            var eventSpeaker = new EventSpeaker
            {
                EventId = dto.EventId,
                SpeakerId = dto.SpeakerId,
                AssignedAt = DateTime.UtcNow
            };
            _context.EventSpeakers.Add(eventSpeaker);
            await _context.SaveChangesAsync();
            return new ServiceResult<bool>
            {
                Success = true,
                Message = "Speaker assigned successfully",
                Data = true
            };
        }
        catch (Exception)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Database error occurred while assigning speaker",
                ErrorType = ErrorType.DatabaseError
            };
        }
    }
    public async Task<ServiceResult<List<EventDetailsDto>>> GetUpcomingEventsAsync()
    {
        try
        {
            var upcomingEvents = await _context.Events
                .Include(e => e.EventSpeakers)
                    .ThenInclude(es => es.Speaker)
                .Include(e => e.EventParticipants)
                .Where(e => e.EventDate > DateTime.UtcNow)
                .OrderBy(e => e.EventDate)
                .Select(e => new EventDetailsDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    MaxParticipants = e.MaxParticipants,
                    RegisteredParticipants = e.EventParticipants.Count(ep => !ep.IsCancelled),
                    AvailableSeats = e.MaxParticipants - e.EventParticipants.Count(ep => !ep.IsCancelled),
                    Speakers = e.EventSpeakers.Select(es => new SpeakerDto
                    {
                        Id = es.Speaker.Id,
                        FirstName = es.Speaker.FirstName,
                        LastName = es.Speaker.LastName
                    }).ToList()
                })
                .ToListAsync();
            return new ServiceResult<List<EventDetailsDto>>
            {
                Success = true,
                Data = upcomingEvents
            };
        }
        catch (Exception)
        {
            return new ServiceResult<List<EventDetailsDto>>
            {
                Success = false,
                Message = "Database error occurred while fetching events",
                ErrorType = ErrorType.DatabaseError
            };
        }
    }
} 