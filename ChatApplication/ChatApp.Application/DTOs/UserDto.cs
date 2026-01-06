using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.DTOs
{
   
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public bool IsOnline { get; set; }
    }

    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }

    // Conversation DTOs
    public class ConversationDto
    {
        public string Id { get; set; }
        public ConversationType Type { get; set; }
        public string Title { get; set; }
        public List<UserDto> Participants { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public string LastMessageContent { get; set; }
        public string LastMessageSenderName { get; set; }
        public int UnreadCount { get; set; }
    }

    public class CreateDirectMessageDto
    {
        public string User1Id { get; set; }
        public string User2Id { get; set; }
    }

    public class CreateGroupChatDto
    {
        public string Title { get; set; }
        public List<string> ParticipantIds { get; set; }
        public string CreatorId { get; set; }
    }

    // Message DTOs
    public class MessageDto
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<ReadReceiptDto> ReadReceipts { get; set; }
        public string ReplyToMessageId { get; set; }
        public bool IsReadByCurrentUser { get; set; }
    }

    public class ReadReceiptDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public DateTime ReadAt { get; set; }
    }

    public class SendMessageDto
    {
        public string ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public string ReplyToMessageId { get; set; }
    }
}
