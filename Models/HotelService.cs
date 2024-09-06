using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class HotelService
{
    public int ServiceId { get; set; }

    public string? ServiceType { get; set; }

    public int? HotelId { get; set; }

    public virtual Hotel? Hotel { get; set; }
}
