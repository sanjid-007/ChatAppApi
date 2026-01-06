using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;

namespace ChatApp.Application.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        public ConversationService(
            IConversationRepository conversationRepository,
            IUserRepository userRepository,
            IMessageRepository messageRepository)
        {
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public async Task<ConversationDto> CreateDirectMessageAsync(CreateDirectMessageDto dto)
        {
            var existing = await _conversationRepository.GetDirectMessageConversationAsync(
                dto.User1Id, dto.User2Id);

            if (existing != null)
                return await MapToDto(existing, dto.User1Id);

            var users = await _userRepository.GetByIdsAsync(new List<string> { dto.User1Id, dto.User2Id });
            if (users.Count() != 2)
                throw new InvalidOperationException("One or both users not found");

            var conversation = Conversation.CreateDirectMessage(dto.User1Id, dto.User2Id);
            var saved = await _conversationRepository.AddAsync(conversation);

            return await MapToDto(saved, dto.User1Id);
        }

        public async Task<ConversationDto> CreateGroupChatAsync(CreateGroupChatDto dto)
        {
            var users = await _userRepository.GetByIdsAsync(dto.ParticipantIds);
            if (users.Count() != dto.ParticipantIds.Count)
                throw new InvalidOperationException("One or more users not found");
            var conversation = Conversation.CreateGroupChat(dto.Title, dto.ParticipantIds, dto.CreatorId);
            var saved = await _conversationRepository.AddAsync(conversation);

            return await MapToDto(saved, dto.CreatorId);
        }

        public async Task<ConversationDto> GetConversationAsync(string conversationId, string currentUserId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (conversation == null)
                throw new InvalidOperationException("Conversation not found");

            if (!conversation.HasParticipant(currentUserId))
                throw new InvalidOperationException("User is not a participant");

            return await MapToDto(conversation, currentUserId);
        }

        public async Task<IEnumerable<ConversationDto>> GetUserConversationsAsync(string userId)
        {
            var conversations = await _conversationRepository.GetUserConversationsAsync(userId);
            var dtos = new List<ConversationDto>();

            foreach (var conversation in conversations)
            {
                dtos.Add(await MapToDto(conversation, userId));
            }

            return dtos.OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt);
        }

        public async Task<bool> AddParticipantAsync(string conversationId, string userId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (conversation == null) return false;

            conversation.AddParticipant(userId);
            return await _conversationRepository.UpdateAsync(conversation);
        }

        public async Task<bool> RemoveParticipantAsync(string conversationId, string userId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (conversation == null) return false;

            conversation.RemoveParticipant(userId);
            return await _conversationRepository.UpdateAsync(conversation);
        }

        public async Task<bool> UpdateGroupTitleAsync(string conversationId, string newTitle)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);
            if (conversation == null) return false;

            conversation.UpdateTitle(newTitle);
            return await _conversationRepository.UpdateAsync(conversation);
        }

        private async Task<ConversationDto> MapToDto(Conversation conversation, string currentUserId)
        {
            var participants = await _userRepository.GetByIdsAsync(conversation.ParticipantIds);
            var unreadCount = await _messageRepository.GetUnreadCountAsync(conversation.Id, currentUserId);

            string title = conversation.Title;
            if (conversation.Type == ConversationType.DirectMessage)
            {
                var otherUserId = conversation.GetOtherParticipantId(currentUserId);
                var otherUser = participants.FirstOrDefault(u => u.Id == otherUserId);
                title = otherUser?.DisplayName ?? "Unknown User";
            }

            return new ConversationDto
            {
                Id = conversation.Id,
                Type = conversation.Type,
                Title = title,
                Participants = participants.Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    DisplayName = u.DisplayName,
                    IsOnline = u.IsOnline,
                    LastSeenAt = u.LastSeenAt
                }).ToList(),
                CreatedAt = conversation.CreatedAt,
                LastMessageAt = conversation.LastMessageAt,
                LastMessageContent = conversation.LastMessageContent,
                UnreadCount = unreadCount
            };
        }
    }
}
