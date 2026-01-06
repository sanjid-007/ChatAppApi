using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class Message
    {
        public string Id { get; private set; }

        public string ConversationId { get; private set; }
        public string SenderId { get; private set; }
        public string Content { get; private set; }
        public MessageType Type { get; private set; }

        public DateTime SentAt { get; private set; }

        public DateTime? EditedAt { get; private set; }

        public bool IsDeleted { get; private set; }

        public List<MessageReadReceipt> ReadReceipts { get; private set; }

        public string ReplyToMessageId { get; private set; }
        private Message() {
            ReadReceipts = new List<MessageReadReceipt>();
        }
        public static Message Create(string conversationId, string senderId, string content, MessageType type)
        {
           return new Message
           {
               ConversationId = conversationId,
               SenderId = senderId,
               Content = content,
               Type = type,
               SentAt = DateTime.UtcNow,
               IsDeleted = false
           };
        }

        public static Message CreateReply(string conversationId, string senderId, string content, MessageType type, string replyToMessageId)
        {
            var message = Create(conversationId, senderId, content, type);
            message.ReplyToMessageId = replyToMessageId;
            return message;

        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = id;
            }
        }

        public void EditContent(string newContent, string editorId)
        {
            Content = newContent;
            EditedAt = DateTime.UtcNow;

        }

        public void Delete(string deletorId)
        {
            IsDeleted = true;
        }

        public void MarkAsReadBy(string userId)
        {
            if (userId == SenderId)
                return; 

            var existingReceipt = ReadReceipts.FirstOrDefault(r => r.UserId == userId);
            if (existingReceipt == null)
            {
                ReadReceipts.Add(new MessageReadReceipt
                {
                    UserId = userId,
                    ReadAt = DateTime.UtcNow
                });
            }
        }

        public bool IsReadBy(string userId)
        {
            return ReadReceipts.Any(r => r.UserId == userId);
        }

        public int GetReadCount()
        {
            return ReadReceipts.Count;
        }

    }

    public class MessageReadReceipt
    {
        public string UserId { get; set; }
        public DateTime ReadAt { get; set; }
    }


    public enum  MessageType
    {
        Text = 0,
        Image = 1,
        File = 2,
        Audio = 3,
        Video = 4,
        System = 5
    }
}
