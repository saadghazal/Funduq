using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class RoomReservation
{
    public int ReservationId { get; set; }

    public int? RoomId { get; set; }

    public int? UserId { get; set; }


    public decimal? TotalPrice { get; set; }

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Room? Room { get; set; }

    public virtual UserProfile? User { get; set; }
}
