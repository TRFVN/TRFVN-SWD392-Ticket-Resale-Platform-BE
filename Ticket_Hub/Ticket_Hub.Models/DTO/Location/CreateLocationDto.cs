using System.ComponentModel.DataAnnotations;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Models.DTO.Location;

public class CreateLocationDto : BaseEntity<string, string, int>
{
    public string City { get; set; } = null!;
    [StringLength(50)]
    public string District { get; set; } = null!;
    [StringLength(50)]
    public string Street { get; set; } = null!;
}