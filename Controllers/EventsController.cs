using EventManagementApi.DTOs;
using EventManagementApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    
    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateEvent([FromBody] CreateEventDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _eventService.CreateEventAsync(dto);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ErrorType.ValidationError => BadRequest(result.Message),
                ErrorType.DatabaseError => StatusCode(500, "Internal server error"),
                _ => StatusCode(500, "An unexpected error occurred")
            };
        }
        return CreatedAtAction(nameof(GetUpcomingEvents), new { id = result.Data }, result.Data);
    }

    /// <summary>
    /// Assign a speaker to an event
    /// </summary>
    [HttpPost("assign-speaker")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> AssignSpeaker([FromBody] AssignSpeakerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _eventService.AssignSpeakerAsync(dto);
        if (!result.Success)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result.Message),
                ErrorType.Conflict => Conflict(result.Message),
                ErrorType.BusinessRuleViolation => UnprocessableEntity(result.Message),
                ErrorType.DatabaseError => StatusCode(500, "Internal server error"),
                _ => StatusCode(500, "An unexpected error occurred")
            };
        }
        return Ok(result.Message);
    }

    /// <summary>
    /// Get all upcoming events with available seats information
    /// </summary>
    [HttpGet("upcoming")]
    [ProducesResponseType(typeof(List<EventDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EventDetailsDto>>> GetUpcomingEvents()
    {
        var result = await _eventService.GetUpcomingEventsAsync();
        if (!result.Success)
        {
            return StatusCode(500, "Internal server error");
        }
        return Ok(result.Data);
    }
} 