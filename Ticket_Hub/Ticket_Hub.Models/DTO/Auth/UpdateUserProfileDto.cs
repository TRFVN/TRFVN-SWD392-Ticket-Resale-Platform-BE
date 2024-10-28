using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Auth
{
    public class UpdateUserProfileDto
    {
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public string Country { get; set; }
        public string Cccd { get; set; }
        public string Address { get; set; }
    }
}
