using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class Testimonial
{
    public int TestimonialId { get; set; }

    public int? UserId { get; set; }

    public int? Rating { get; set; }

    public string? UserOpinion { get; set; }

    public string TestimonialStatus { get; set; }

    public virtual UserProfile? User { get; set; }
}
