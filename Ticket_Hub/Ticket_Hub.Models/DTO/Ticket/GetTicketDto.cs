﻿using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.Ticket;

public class GetTicketDto
{
    public Guid TicketId { get; set; }
    public string TicketName { get; set; } = null!;
    public Guid EventId { get; set; }
    public string UserId { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public double TicketPrice { get; set; }
    public int TicketQuantity { get; set; }
    public string TicketDescription { get; set; } = null!;
    public string SerialNumber { get; set; } = null!;
    public int Status { get; set; }
}