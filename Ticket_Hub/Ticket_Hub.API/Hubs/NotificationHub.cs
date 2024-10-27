using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace Ticket_Hub.Models.DTO.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Gửi tin nhắn đến tất cả các client đã kết nối
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        
        public async Task SendPrivateMessage(string receiverConnectionId, string message)
        {
            // Gửi tin nhắn riêng tư đến client có connectionId xác định
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        // Từ điển lưu trữ UserId và ConnectionId của họ
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();
        
        public override Task OnConnectedAsync()
        {
            // Ghi lại ConnectionId khi người dùng kết nối
            // Code lưu trữ thông tin về user và connectionId 
            string userId = Context.UserIdentifier; // Xác định UserId của người dùng hiện tại (sau khi đăng nhập)
            string connectionId = Context.ConnectionId;

            // Thêm hoặc cập nhật ConnectionId của UserId vào từ điển
            UserConnections[userId] = connectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Xử lý khi người dùng ngắt kết nối
            // Xóa thông tin user và connectionId 
            string userId = Context.UserIdentifier;

            // Xóa ConnectionId của UserId khi ngắt kết nối
            UserConnections.TryRemove(userId, out _);
            return base.OnDisconnectedAsync(exception);
        }
        
        public static string GetConnectionId(string userId)
        {
            // Lấy ConnectionId dựa trên UserId
            UserConnections.TryGetValue(userId, out string connectionId);
            return connectionId;
        }
    }
}
