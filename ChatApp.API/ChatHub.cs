using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatApp.API
{
    public class ChatHub : Hub
    {
        // Gửi tin nhắn tới 1 conversation (group)
        public async Task SendMessage(string conversationId, string senderId, string message)
        {
            // Broadcast tới tất cả client trong nhóm
            await Clients.Group(conversationId).SendAsync("ReceiveMessage", senderId, message);
        }

        // Tham gia vào 1 conversation (group)
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        // Rời khỏi 1 conversation (group)
        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }

        // Gửi notification tới user
        public async Task SendNotification(string userId, string notification)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }

        // Gửi reaction cho 1 message
        public async Task SendReaction(string conversationId, string messageId, string userId, string reaction)
        {
            await Clients.Group(conversationId).SendAsync("ReceiveReaction", messageId, userId, reaction);
        }
    }
}
