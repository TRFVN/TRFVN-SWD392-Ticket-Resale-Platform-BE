using System.ComponentModel.DataAnnotations;
using Ticket_Hub.Models.Models;

    public class GetLocationDto : BaseEntity<string, string , int>
    {
        public Guid LocationId { get; set; }
        [StringLength(50)] 
        public string City { get; set; } = null!;
        [StringLength(50)]
        public string District { get; set; } = null!;
        [StringLength(50)]
        public string Street { get; set; } = null!;

    }