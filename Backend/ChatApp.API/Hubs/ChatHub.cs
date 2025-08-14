using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using ChatApp.Application.UseCases;
using ChatApp.Application.UseCases.Chat.Commands;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Presentation.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ChatHub> _logger;
        private readonly UserSession<UserProfile> _userSession;

        public ChatHub(IMediator mediator, IIdentityService identityService, ILogger<ChatHub> logger)
        {
            _mediator = mediator;
            _identityService = identityService;
            _logger = logger;
            _userSession = _identityService.GetUser<UserProfile>();
        }

        // Gửi tin nhắn real-time
        public async Task SendMessage(long chatId, string content, MessageType messageType)
        {
            try
            {
                var command = new SendMessageCommand
                {
                    ChatID = chatId,
                    Content = content,
                    MessageType = messageType,
                };

                var result = await _mediator.Send(command);

                var message = new Message
                {
                    ID = result.Data,
                    ChatID = chatId,
                    SenderID = _userSession.Data.UserID,
                    Content = content,
                    MessageType = messageType,
                    IsDeleted = false,
                    IsEdited = false,
                    CreatedAt = DateTime.UtcNow,
                    EditedAt = null,
                    DeletedAt = null,
                    SenderName = $"{_userSession.Data.FirstName} {_userSession.Data.LastName}",
                    SenderAvatar = _userSession.Data.Avatar
                };

                await Clients.Group($"Chat_{chatId}").SendAsync("ReceiveMessage", message);

                _logger.LogInformation($"Message sent to chat {chatId} by user {_userSession.Data.UserID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                throw new HubException($"Failed to send message: {ex.Message}");
            }
        }

        // Thêm reaction real-time
        public async Task AddReaction(long messageId, string emoji)
        {
            try
            {
                var command = new AddReactionCommand
                {
                    MessageID = messageId,
                    Emoji = emoji
                };

                var reaction = await _mediator.Send(command);
                await Clients.Group($"Chat_{messageId}").SendAsync("ReceiveReaction", reaction);
                _logger.LogInformation($"Reaction added to message {messageId} by user {_userSession.Data.UserID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reaction");
                throw new HubException($"Failed to add reaction: {ex.Message}");
            }
        }

        // Tham gia chat
        // public async Task JoinChat(long chatId)
        // {
        //     try
        //     {
        //         var userSession = _identityService.GetUser<UserProfile>();

        //         var chatParticipant = await _chatParticipantRepository.GetByChatIdAndUserIdAsync(chatId, userSession.Data.UserID);
        //         if (chatParticipant == null)
        //             throw new HubException("User is not a participant of this chat");

        //         await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        //         await Clients.Group($"Chat_{chatId}").SendAsync("UserJoined", userSession.Data.UserID);
        //         _logger.LogInformation($"User {userSession.Data.UserID} joined chat {chatId}");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error joining chat");
        //         throw new HubException($"Failed to join chat: {ex.Message}");
        //     }
        // }

        // Rời chat
        public async Task LeaveChat(long chatId)
        {
            try
            {
                if (_userSession == null || _userSession.Data.UserID == 0)
                {
                    throw new HubException("User is not authenticated");
                }

                var command = new LeaveChatCommand()
                {
                    ChatID = chatId
                };

                var status = await _mediator.Send(command);
                if (status.Code != 1)
                {
                    throw new HubException("Failed to leave chat");
                }
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
                await Clients.Group($"Chat_{chatId}").SendAsync("UserLeft", _userSession.Data.UserID);
                _logger.LogInformation($"User {_userSession.Data.UserID} left chat {chatId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error leaving chat");
                throw new HubException($"Failed to leave chat: {ex.Message}");
            }
        }

        // Cập nhật trạng thái online khi kết nối
        public override async Task OnConnectedAsync()
        {
            try
            {
                if (_userSession == null || _userSession.Data.UserID == 0)
                {
                    throw new HubException("User is not authenticated");
                }

                var command = new UpdateUserCommand
                {
                    IsOnline = true,
                    LastSeen = DateTime.UtcNow
                };

                var status = await _mediator.Send(command);
                if (status.Code != 1)
                {
                    throw new HubException("Failed to update user");
                }

                if (status.Code == 1)
                {
                    await Clients.All.SendAsync("UserOnline", _userSession.Data.UserID);
                }

                _logger.LogInformation($"User {_userSession.Data.FullName} connected and set to online");
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on connection");
                throw new HubException($"Connection failed: {ex.Message}");
            }
        }

        // Cập nhật trạng thái offline khi ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                if (_userSession == null || _userSession.Data.UserID == 0)
                {
                    throw new HubException("User is not authenticated");
                }

                var command = new UpdateUserCommand
                {
                    IsOnline = false,
                    LastSeen = DateTime.UtcNow
                };

                var status = await _mediator.Send(command);
                if (status.Code != 1)
                {
                    throw new HubException("Failed to update user");
                }

                if (status.Code == 1)
                {
                    await Clients.All.SendAsync("UserOffline", _userSession.Data.UserID);
                }

                _logger.LogInformation($"User {_userSession.Data.FullName} disconnected and set to offline");

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on disconnection");
                throw new HubException($"Disconnection failed: {ex.Message}");
            }
        }
    }
}