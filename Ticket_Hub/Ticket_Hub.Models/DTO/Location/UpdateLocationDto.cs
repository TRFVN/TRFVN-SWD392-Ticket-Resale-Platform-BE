using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.Location;

public class UpdateLocationDto
{
    public Guid LocationId { get; set; }
    public string City { get; set; } = null!;
    [StringLength(50)]
    public string District { get; set; } = null!;
    [StringLength(50)]
    public string Street { get; set; } = null!;
}