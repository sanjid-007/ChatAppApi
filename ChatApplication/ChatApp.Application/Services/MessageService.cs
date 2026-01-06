

using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;


namespace ChatApp.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRepository _userRepository;

        public MessageService(
            IMessageRepository messageRepository,
            IConversationRepository conversationRepository,
            IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
        }

        public async Task<MessageDto> SendMessageAsync(SendMessageDto dto)
        {
            // Validate conversation exists and user is participant
            var conversation = await _conversationRepository.GetByIdAsync(dto.ConversationId);
            if (conversation == null)
                throw new InvalidOperationException("Conversation not found");

            if (!conversation.HasParticipant(dto.SenderId))
                throw new InvalidOperationException("User is not a participant");

            // Create message
            Message message;
            if (!string.IsNullOrWhiteSpace(dto.ReplyToMessageId))
            {
                message = Message.CreateReply(dto.ConversationId, dto.SenderId, dto.Content,dto.Type, dto.ReplyToMessageId);
            }
            else
            {
                message = Message.Create(dto.ConversationId, dto.SenderId, dto.Content, dto.Type);
            }

            var saved = await _messageRepository.AddAsync(message);

            // Update conversation's last message
            conversation.UpdateLastMessage(saved.Content, saved.SenderId);
            await _conversationRepository.UpdateAsync(conversation);

            return await MapToDto(saved, dto.SenderId);
        }

        public async Task<IEnumerable<MessageDto>> GetConversationMessagesAsync(
            string conversationId,
            string currentUserId,
            int skip = 0,
            int take = 50)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (conversation == null)
                throw new InvalidOperationException("Conversation not found");

            if (!conversation.HasParticipant(currentUserId))
                throw new InvalidOperationException("User is not a participant");

            var messages = await _messageRepository.GetConversationMessagesAsync(conversationId, skip, take);
            var dtos = new List<MessageDto>();

            foreach (var message in messages)
            {
                dtos.Add(await MapToDto(message, currentUserId));
            }

            return dtos;
        }

        public async Task<MessageDto> EditMessageAsync(string messageId, string newContent, string editorId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null)
                throw new InvalidOperationException("Message not found");

            message.EditContent(newContent, editorId);
            await _messageRepository.UpdateAsync(message);

            return await MapToDto(message, editorId);
        }

        public async Task<bool> DeleteMessageAsync(string messageId, string deleterId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null) return false;

            message.Delete(deleterId);
            return await _messageRepository.UpdateAsync(message);
        }

        public async Task<bool> MarkAsReadAsync(string messageId, string userId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null) return false;

            message.MarkAsReadBy(userId);
            return await _messageRepository.UpdateAsync(message);
        }

        public async Task<int> GetUnreadCountAsync(string conversationId, string userId)
        {
            return await _messageRepository.GetUnreadCountAsync(conversationId, userId);
        }

        private async Task<MessageDto> MapToDto(Message message, string currentUserId)
        {
            var sender = await _userRepository.GetByIdAsync(message.SenderId);

            return new MessageDto
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId,
                SenderName = sender?.DisplayName ?? "Unknown",
                Content = message.Content,
                Type = message.Type,
                SentAt = message.SentAt,
                EditedAt = message.EditedAt,
                IsDeleted = message.IsDeleted,
                ReplyToMessageId = message.ReplyToMessageId,
                IsReadByCurrentUser = message.IsReadBy(currentUserId),
                ReadReceipts = message.ReadReceipts.Select(r => new ReadReceiptDto
                {
                    UserId = r.UserId,
                    ReadAt = r.ReadAt
                }).ToList()
            };
        }
    }
}
