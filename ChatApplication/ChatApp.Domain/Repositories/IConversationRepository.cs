using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Repositories
{
    public interface IConversationRepository
    {
        Task<Conversation> GetByIdAsync(string id);
        Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId);
        Task<Conversation> GetDirectMessageConversationAsync(string user1Id, string user2Id);
        Task<Conversation> AddAsync(Conversation conversation);
        Task<bool> UpdateAsync(Conversation conversation);
        Task<bool> DeleteAsync(string id);
    }
}
