using LibraryApi.DTOs;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILibraryService _libraryService;
    
    public UsersController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDto>> GetUser(int id)
    {
        var user = await _libraryService.GetUserDetailsAsync(id);
        
        if (user == null)
            return NotFound($"User with ID {id} not found");  // 404
        
        return Ok(user);  // 200
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] UserUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);  // 400
        
        var result = await _libraryService.UpdateUserAsync(id, updateDto);
        
        if (!result)
            return NotFound($"User with ID {id} not found or invalid book IDs provided");  // 404
        
        return Ok("Aktualizacja powiodła się");  // 200
    }
} 