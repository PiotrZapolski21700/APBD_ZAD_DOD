using EventManagementApi.Data;
using EventManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeakersController : ControllerBase
{
    private readonly EventDbContext _context;
    
    public SpeakersController(EventDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    [ProducesResponseType(typeof(List<Speaker>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Speaker>>> GetSpeakers()
    {
        var speakers = await _context.Speakers
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync();
        return Ok(speakers);
    }
} 