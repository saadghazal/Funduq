using System;
using System.Collections.Generic;

namespace funduq.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleType { get; set; }

    public virtual ICollection<UserCredential> UserCredentials { get; set; } = new List<UserCredential>();
}
