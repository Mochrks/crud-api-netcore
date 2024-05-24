using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public DateOnly OrderDate { get; set; }

    public int TotalItem { get; set; }

    public int TotalOrderPrice { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
