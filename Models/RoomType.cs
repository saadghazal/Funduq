using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class RoomType
{
    public int TypeId { get; set; }

    public string? RoomType1 { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
