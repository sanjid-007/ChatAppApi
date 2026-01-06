using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs;

namespace ChatApp.Application.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> SendMessageAsync(SendMessageDto dto);
        Task<IEnumerable<MessageDto>> GetConversationMessagesAsync(string conversationId, string currentUserId, int skip = 0, int take = 50);
        Task<MessageDto> EditMessageAsync(string messageId, string newContent, string editorId);
        Task<bool> DeleteMessageAsync(string messageId, string deleterId);
        Task<bool> MarkAsReadAsync(string messageId, string userId);
        Task<int> GetUnreadCountAsync(string conversationId, string userId);
    }
}
