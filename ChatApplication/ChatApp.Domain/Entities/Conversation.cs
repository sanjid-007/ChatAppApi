using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class Conversation
    {
        public string Id { get; private set; }

        public string Title { get; private set; }

        public ConversationType Type { get; private set; }

        public List<string> ParticipantIds { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime LastMessageAt { get; private set; }
        public string LastMessageContent { get; private set; }

        public string LastMessageSenderId { get; private set; }

        private Conversation()
        {
            ParticipantIds = new List<string>();
        }

        public static Conversation CreateDirectMessage(string user1Id, string user2Id)
        {
            return new Conversation
            {
                Type = ConversationType.DirectMessage,
                ParticipantIds = new List<string> { user1Id, user2Id },
                CreatedAt = DateTime.UtcNow
            };
        }

        public static Conversation CreateGroupChat(string title, List<string> participantIds, string creatorId)
        {
            if (!participantIds.Contains(creatorId))
            {
                participantIds.Add(creatorId);
            }
            return new Conversation
            {
                Title = title,
                Type = ConversationType.GroupChat,
                ParticipantIds = participantIds,
                CreatedAt = DateTime.UtcNow
            };
        }
        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = id;
            }
        }

        public void AddParticipant(string userId)
        {
            if (!ParticipantIds.Contains(userId))
            {
                ParticipantIds.Add(userId);
            }
        }
        public void RemoveParticipant(string userId)
        {
            if (ParticipantIds.Contains(userId))
            {
                ParticipantIds.Remove(userId);
            }

        }
        public void UpdateLastMessage(string messageContent, string senderId)
        {
            LastMessageContent = messageContent;
            LastMessageSenderId = senderId;
            LastMessageAt = DateTime.UtcNow;
        }

        public void UpdateTitle(string newTitle)
        {
            if (Type == ConversationType.GroupChat)
            {
                Title = newTitle;
            }
        }

        public bool HasParticipant(string userId)
        {
            return ParticipantIds.Contains(userId);
        }

        public string GetOtherParticipantId(string userId)
        {
            if (Type != ConversationType.DirectMessage)
            {
                throw new InvalidOperationException("This method is only valid for direct message conversations.");
            }
            return ParticipantIds.FirstOrDefault(id => id != userId);
        }
    }

    public enum ConversationType
    {
        DirectMessage = 0,
        GroupChat = 1
    }
}
