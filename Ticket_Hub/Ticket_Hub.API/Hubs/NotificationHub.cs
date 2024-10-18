using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Hubs
{
    public class NotificationHub : Hub
    {
        // Phương thức này được client gọi để gửi tin nhắn đến tất cả các client khác
        public async Task SendMessage(string user, string message)
        {
            // Gửi tin nhắn đến tất cả các client đang kết nối
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
