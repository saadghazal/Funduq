using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class EventReservation
{
    public int ReservationId { get; set; }

    public int? EventId { get; set; }

    public int? UserId { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateOnly? EventDay { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual SpecialEvent? Event { get; set; }

    public virtual UserProfile? User { get; set; }
}
