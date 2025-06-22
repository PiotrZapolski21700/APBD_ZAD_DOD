using EventManagementApi.DTOs;
using EventManagementApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantService _participantService;
    
    public ParticipantsController(IParticipantService participantService)
    {
        _participantService = participantService;
    }
    [HttpPost("register")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<int>> RegisterParticipant([FromBody] RegisterParticipantDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _participantService.RegisterParticipantAsync(dto);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result.Message),
                ErrorType.BusinessRuleViolation => UnprocessableEntity(result.Message),
                ErrorType.Conflict => Conflict(result.Message),
                ErrorType.DatabaseError => StatusCode(500, "Internal server error"),
                _ => StatusCode(500, "An unexpected error occurred")
            };
        }
        return CreatedAtAction(nameof(GetParticipantReport), new { participantId = result.Data }, result.Data);
    }
    [HttpDelete("cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> CancelRegistration([FromQuery] int eventId, [FromQuery] int participantId)
    {
        if (eventId <= 0 || participantId <= 0)
        {
            return BadRequest("Invalid event ID or participant ID");
        }
        var result = await _participantService.CancelRegistrationAsync(eventId, participantId);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result.Message),
                ErrorType.BusinessRuleViolation => UnprocessableEntity(result.Message),
                ErrorType.DatabaseError => StatusCode(500, "Internal server error"),
                _ => StatusCode(500, "An unexpected error occurred")
            };
        }
        return Ok(result.Message);
    }
    [HttpGet("{participantId}/report")]
    [ProducesResponseType(typeof(ParticipantReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ParticipantReportDto>> GetParticipantReport(int participantId)
    {
        if (participantId <= 0)
        {
            return BadRequest("Invalid participant ID");
        }
        var result = await _participantService.GetParticipantReportAsync(participantId);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result.Message),
                ErrorType.DatabaseError => StatusCode(500, "Internal server error"),
                _ => StatusCode(500, "An unexpected error occurred")
            };
        }
        return Ok(result.Data);
    }
} 