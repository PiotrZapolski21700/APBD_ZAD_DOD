using LibraryApi.DTOs;
using System.Threading.Tasks;

namespace LibraryApi.Services;

public interface ILibraryService
{
    Task<UserDetailsDto?> GetUserDetailsAsync(int userId);
    Task<bool> UpdateUserAsync(int userId, UserUpdateDto updateDto);
} 