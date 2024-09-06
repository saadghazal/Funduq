using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace funduq.Models;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ProfilePicture { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? CreatedAt { get; set; }


    [NotMapped]
    public virtual IFormFile ProfileImage { get; set; }


    public virtual ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();

    public virtual ICollection<EventReservation> EventReservations { get; set; } = new List<EventReservation>();

    public virtual ICollection<RoomReservation> RoomReservations { get; set; } = new List<RoomReservation>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<UserCredential> UserCredentials { get; set; } = new List<UserCredential>();
}
