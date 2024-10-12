using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket_Hub.Models.Models
{
    public class Event : BaseEntity<string, string, int>
    {
        [Key] public Guid EventId { get; set; }
        [StringLength(100)] public string EventName { get; set; } = null!;
        [StringLength(500)] public string EventDescription { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public Guid LocationId { get; set; }
        [ForeignKey("LocationId")] public virtual Location Location { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = null!;
    }
}