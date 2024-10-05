using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.Models
{
    public class Event
    {
        [Key] public Guid EventId { get; set; }
        [StringLength(100)] public string EventName { get; set; } = null!;
        [StringLength(500)] public string EventDescription { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public Guid LocationId { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}