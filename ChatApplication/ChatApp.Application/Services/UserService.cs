using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;

namespace ChatApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var existingByUsername = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingByUsername != null)
                throw new InvalidOperationException("Username already exists");

            var existingByEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingByEmail != null)
                throw new InvalidOperationException("Email already exists");

            var user = User.Create(dto.Username, dto.Email, dto.DisplayName);
            var saved = await _userRepository.AddAsync(user);

            return MapToDto(saved);
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm);
            return users.Select(MapToDto);
        }

        public async Task<bool> UpdateDisplayNameAsync(string userId, string displayName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.UpdateDisplayName(displayName);
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> UpdateOnlineStatusAsync(string userId, bool isOnline)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.SetOnlineStatus(isOnline);
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await _userRepository.DeleteAsync(userId);
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName,
                CreatedAt = user.CreatedAt,
                LastSeenAt = user.LastSeenAt,
                IsOnline = user.IsOnline
            };
        }
    }
}
