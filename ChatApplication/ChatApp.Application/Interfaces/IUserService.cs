using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs;

namespace ChatApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto> GetUserByIdAsync(string id);
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm);
        Task<bool> UpdateDisplayNameAsync(string userId, string displayName);
        Task<bool> UpdateOnlineStatusAsync(string userId, bool isOnline);
        Task<bool> DeleteUserAsync(string userId);
    }
}
