using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs;

namespace ChatApp.Application.Interfaces
{
    public interface IConversationService
    {
        Task<ConversationDto> CreateDirectMessageAsync(CreateDirectMessageDto dto);
        Task<ConversationDto> CreateGroupChatAsync(CreateGroupChatDto dto);
        Task<ConversationDto> GetConversationAsync(string conversationId, string currentUserId);
        Task<IEnumerable<ConversationDto>> GetUserConversationsAsync(string userId);
        Task<bool> AddParticipantAsync(string conversationId, string userId);
        Task<bool> RemoveParticipantAsync(string conversationId, string userId);
        Task<bool> UpdateGroupTitleAsync(string conversationId, string newTitle);
    }
}
