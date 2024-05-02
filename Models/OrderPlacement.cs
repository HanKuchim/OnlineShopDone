using System;
using System.Collections.Generic;

namespace OnlineShop.Models;

public partial class OrderPlacement
{
    public int Id { get; set; }

    public int? WorkerId { get; set; }

    public int? OrderId { get; set; }

    public DateTime? OrderPlacementDate { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Worker? Worker { get; set; }
}
