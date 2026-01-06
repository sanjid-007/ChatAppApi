using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> GetByIdAsync(string id);
        Task<IEnumerable<Message>> GetConversationMessagesAsync(string conversationId, int skip = 0, int take = 50);
        Task<IEnumerable<Message>> GetUnreadMessagesAsync(string conversationId, string userId);
        Task<Message> AddAsync(Message message);
        Task<bool> UpdateAsync(Message message);
        Task<bool> DeleteAsync(string id);
        Task<int> GetUnreadCountAsync(string conversationId, string userId);
    }
}
