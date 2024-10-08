using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.Models
{
    public class Location
    {
        [Key]
        public Guid LocationId { get; set; }
        [StringLength(50)] 
        public string City { get; set; } = null!;
        [StringLength(50)]
        public string District { get; set; } = null!;
        [StringLength(50)]
        public string Street { get; set; } = null!;

    }
}
