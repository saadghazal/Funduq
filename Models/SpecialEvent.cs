using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class SpecialEvent
{
    public int EventId { get; set; }

    public int? HotelId { get; set; }

    public string? EventDescription { get; set; }

    public string? EventPicture { get; set; }

    public decimal? TicketPrice { get; set; }

    public int? MaximumTickets { get; set; }

    public DateOnly? AvailableFrom { get; set; }

    public DateOnly? AvailableTo { get; set; }

    public virtual ICollection<EventReservation> EventReservations { get; set; } = new List<EventReservation>();

    public virtual Hotel? Hotel { get; set; }
}
