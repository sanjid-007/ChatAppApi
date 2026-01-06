using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ChatApp.Application.Interfaces;
using ChatApp.Application.Services;
using ChatApp.Application.DTOs;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationsController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost("direct")]
        public async Task<ActionResult<ConversationDto>> CreateDirectMessage([FromBody] CreateDirectMessageDto dto)
        {
            try
            {
                var result = await _conversationService.CreateDirectMessageAsync(dto);
                return CreatedAtAction(nameof(GetConversation), new { id = result.Id, userId = dto.User1Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("group")]
        public async Task<ActionResult<ConversationDto>> CreateGroupChat([FromBody] CreateGroupChatDto dto)
        {
            try
            {
                var result = await _conversationService.CreateGroupChatAsync(dto);
                return CreatedAtAction(nameof(GetConversation), new { id = result.Id, userId = dto.CreatorId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConversationDto>> GetConversation(string id, [FromQuery] string userId)
        {
            try
            {
                var result = await _conversationService.GetConversationAsync(id, userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ConversationDto>>> GetUserConversations(string userId)
        {
            var result = await _conversationService.GetUserConversationsAsync(userId);
            return Ok(result);
        }

        [HttpPost("{id}/participants")]
        public async Task<ActionResult> AddParticipant(string id, [FromBody] string userId)
        {
            try
            {
                var result = await _conversationService.AddParticipantAsync(id, userId);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}/participants/{userId}")]
        public async Task<ActionResult> RemoveParticipant(string id, string userId)
        {
            try
            {
                var result = await _conversationService.RemoveParticipantAsync(id, userId);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
