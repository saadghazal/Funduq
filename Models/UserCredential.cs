using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class UserCredential
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual UserProfile? User { get; set; }
}
