using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class CreditCard
{
    public int CardId { get; set; }

    public int? UserId { get; set; }

    public string? CardNumber { get; set; }

    public string? CardCvv { get; set; }

    public DateOnly? CardExpiryDate { get; set; }

    public decimal? CardAmount { get; set; }

    public virtual UserProfile? User { get; set; }
}
