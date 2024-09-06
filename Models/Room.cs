using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace funduq.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int? TypeId { get; set; }

    public int? HotelId { get; set; }

    public string? RoomName { get; set; }

    public int? NumberOfRooms { get; set; }

    public decimal? NightPrice { get; set; }

    public string? RoomDescription { get; set; }

    public int? NumberOfBeds { get; set; }

    public string? RoomImage { get; set; }

    public bool? IsAvailable { get; set; } = true;

    public bool? HasOffer { get; set; } = false;

    public bool? IsBreakfastIncluded { get; set; }

    public virtual Hotel? Hotel { get; set; }

    [NotMapped]
    public virtual IFormFile RoomImageFile { get; set; }

    public virtual ICollection<RoomReservation> RoomReservations { get; set; } = new List<RoomReservation>();

    public virtual RoomType? Type { get; set; }

    
}
