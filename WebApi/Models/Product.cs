using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string NameProduct { get; set; } = null!;

    public int Price { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
