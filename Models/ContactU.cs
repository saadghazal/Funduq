using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class ContactU
{
    public int ContactId { get; set; }

    public string? ContactUsPhoneNumber { get; set; }

    public string? ContactUsEmail { get; set; }

    public string? ContactUsAddress { get; set; }

    public string ContactUsOpenDays { get; set; } = null!;

    public string ContactUsOpenHours { get; set; } = null!;

    public virtual ICollection<HomePage> HomePages { get; set; } = new List<HomePage>();
}
