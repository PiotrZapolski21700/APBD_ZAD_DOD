using EventManagementApi.DTOs;

namespace EventManagementApi.Services;

public interface IParticipantService
{
    Task<ServiceResult<int>> RegisterParticipantAsync(RegisterParticipantDto dto);
    Task<ServiceResult<bool>> CancelRegistrationAsync(int eventId, int participantId);
    Task<ServiceResult<ParticipantReportDto>> GetParticipantReportAsync(int participantId);
} 