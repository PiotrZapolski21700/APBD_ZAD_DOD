using EventManagementApi.Data;
using EventManagementApi.DTOs;
using EventManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApi.Services;

public class ParticipantService : IParticipantService
{
    private readonly EventDbContext _context;
    
    public ParticipantService(EventDbContext context)
    {
        _context = context;
    }
    
    public async Task<ServiceResult<int>> RegisterParticipantAsync(RegisterParticipantDto dto)
    {
        var eventEntity = await _context.Events
            .Include(e => e.EventParticipants)
            .FirstOrDefaultAsync(e => e.Id == dto.EventId);
        if (eventEntity == null)
        {
            return new ServiceResult<int>
            {
                Success = false,
                Message = $"Event with ID {dto.EventId} not found",
                ErrorType = ErrorType.NotFound
            };
        }
        if (eventEntity.EventDate <= DateTime.UtcNow)
        {
            return new ServiceResult<int>
            {
                Success = false,
                Message = "Cannot register for past events",
                ErrorType = ErrorType.BusinessRuleViolation
            };
        }
        var registeredCount = eventEntity.EventParticipants.Count(ep => !ep.IsCancelled);
        if (registeredCount >= eventEntity.MaxParticipants)
        {
            return new ServiceResult<int>
            {
                Success = false,
                Message = "Event is fully booked",
                ErrorType = ErrorType.BusinessRuleViolation
            };
        }
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.Email == dto.Email);
            if (participant == null)
            {
                participant = new Participant
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Participants.Add(participant);
                await _context.SaveChangesAsync();
            }
            var existingRegistration = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == dto.EventId && 
                                         ep.ParticipantId == participant.Id);
            if (existingRegistration != null)
            {
                if (!existingRegistration.IsCancelled)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResult<int>
                    {
                        Success = false,
                        Message = "Participant is already registered for this event",
                        ErrorType = ErrorType.Conflict
                    };
                }
                existingRegistration.IsCancelled = false;
                existingRegistration.CancellationDate = null;
                existingRegistration.RegistrationDate = DateTime.UtcNow;
            }
            else
            {
                var registration = new EventParticipant
                {
                    EventId = dto.EventId,
                    ParticipantId = participant.Id,
                    RegistrationDate = DateTime.UtcNow,
                    IsCancelled = false
                };
                _context.EventParticipants.Add(registration);
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return new ServiceResult<int>
            {
                Success = true,
                Message = "Registration successful",
                Data = participant.Id
            };
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return new ServiceResult<int>
            {
                Success = false,
                Message = "Database error occurred during registration",
                ErrorType = ErrorType.DatabaseError
            };
        }
    }
    public async Task<ServiceResult<bool>> CancelRegistrationAsync(int eventId, int participantId)
    {
        var registration = await _context.EventParticipants
            .Include(ep => ep.Event)
            .FirstOrDefaultAsync(ep => ep.EventId == eventId && 
                                     ep.ParticipantId == participantId);
        if (registration == null)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Registration not found",
                ErrorType = ErrorType.NotFound
            };
        }
        if (registration.IsCancelled)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Registration is already cancelled",
                ErrorType = ErrorType.BusinessRuleViolation
            };
        }
        var hoursUntilEvent = (registration.Event.EventDate - DateTime.UtcNow).TotalHours;
        if (hoursUntilEvent < 24)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Cannot cancel registration less than 24 hours before the event",
                ErrorType = ErrorType.BusinessRuleViolation
            };
        }
        try
        {
            registration.IsCancelled = true;
            registration.CancellationDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new ServiceResult<bool>
            {
                Success = true,
                Message = "Registration cancelled successfully",
                Data = true
            };
        }
        catch (Exception)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Database error occurred while cancelling registration",
                ErrorType = ErrorType.DatabaseError
            };
        }
    }
    public async Task<ServiceResult<ParticipantReportDto>> GetParticipantReportAsync(int participantId)
    {
        var participant = await _context.Participants
            .Include(p => p.EventParticipants)
                .ThenInclude(ep => ep.Event)
                    .ThenInclude(e => e.EventSpeakers)
                        .ThenInclude(es => es.Speaker)
            .FirstOrDefaultAsync(p => p.Id == participantId);
        if (participant == null)
        {
            return new ServiceResult<ParticipantReportDto>
            {
                Success = false,
                Message = $"Participant with ID {participantId} not found",
                ErrorType = ErrorType.NotFound
            };
        }
        var report = new ParticipantReportDto
        {
            ParticipantId = participant.Id,
            FirstName = participant.FirstName,
            LastName = participant.LastName,
            Email = participant.Email,
            Events = participant.EventParticipants
                .Where(ep => !ep.IsCancelled && ep.Event.EventDate < DateTime.UtcNow)
                .Select(ep => new ParticipantEventDto
                {
                    EventId = ep.Event.Id,
                    Title = ep.Event.Title,
                    EventDate = ep.Event.EventDate,
                    RegistrationDate = ep.RegistrationDate,
                    IsCancelled = ep.IsCancelled,
                    CancellationDate = ep.CancellationDate,
                    Speakers = ep.Event.EventSpeakers
                        .Select(es => $"{es.Speaker.FirstName} {es.Speaker.LastName}")
                        .ToList()
                })
                .OrderByDescending(e => e.EventDate)
                .ToList()
        };
        return new ServiceResult<ParticipantReportDto>
        {
            Success = true,
            Data = report
        };
    }
} 