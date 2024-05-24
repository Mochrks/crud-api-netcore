using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Fullname { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public DateTime? CreatedTime { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
