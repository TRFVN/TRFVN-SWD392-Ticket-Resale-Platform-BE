using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ticket_Hub.Models.DTO.Auth
{
    public class GoogleUserInfo
    {
        public string email { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string sub { get; set; }

    }
}
