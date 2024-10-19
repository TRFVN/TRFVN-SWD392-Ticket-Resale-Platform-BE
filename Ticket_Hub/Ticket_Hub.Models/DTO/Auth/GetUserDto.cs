using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Auth
{
    public class GetUserDto
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Cccd { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string AvatarUrl { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
