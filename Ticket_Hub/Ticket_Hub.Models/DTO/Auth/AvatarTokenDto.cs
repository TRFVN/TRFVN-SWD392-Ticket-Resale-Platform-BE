using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Auth
{
    public class AvatarTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string AvatarUrl { get; set; }
    }
}
