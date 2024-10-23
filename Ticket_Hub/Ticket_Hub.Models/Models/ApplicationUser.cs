using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Ticket_Hub.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(30)] public string FullName { get; set; } = null!;
        public DateTime BirthDate { get; set; } 
        [StringLength(100)] public string AvatarUrl { get; set; } = null!;
        [StringLength(50)] public string Country { get; set; } = null!;
        [StringLength(12)] public string? Cccd { get; set; }
        [StringLength(100)] public string? Address { get; set; }


        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<CartHeader> CartHeaders { get; set; } = new List<CartHeader>();
        public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}