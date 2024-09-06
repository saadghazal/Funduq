using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace funduq.Models;

public partial class Hotel
{
    public int HotelId { get; set; }

    public string? HotelName { get; set; }

    public string? HotelPicture { get; set; }

    public int? HotelCity { get; set; }

    public int? NumberOfStars { get; set; }

    public bool? IsFeaturedHotel { get; set; }

    [NotMapped]
    public virtual IFormFile HotelImage { get; set; }
    public virtual City? HotelCityNavigation { get; set; }


    public virtual ICollection<HotelService> HotelServices { get; set; } = new List<HotelService>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<SpecialEvent> SpecialEvents { get; set; } = new List<SpecialEvent>();
}
