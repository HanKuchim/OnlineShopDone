using System;
using System.Collections.Generic;

namespace OnlineShop.Models;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string? Adress { get; set; }

    public decimal? Rating { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
