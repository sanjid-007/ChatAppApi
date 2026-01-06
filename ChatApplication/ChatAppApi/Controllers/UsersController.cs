using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto dto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers([FromQuery] string term)
        {
            var users = await _userService.SearchUsersAsync(term);
            return Ok(users);
        }

        [HttpPut("{id}/display-name")]
        public async Task<ActionResult> UpdateDisplayName(string id, [FromBody] string displayName)
        {
            var result = await _userService.UpdateDisplayNameAsync(id, displayName);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/online-status")]
        public async Task<ActionResult> UpdateOnlineStatus(string id, [FromBody] bool isOnline)
        {
            var result = await _userService.UpdateOnlineStatusAsync(id, isOnline);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
