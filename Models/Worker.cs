using System;
using System.Collections.Generic;

namespace OnlineShop.Models;

public partial class Worker
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Salary { get; set; }

    public int? Jobtitleid { get; set; }

    public int? PickupPoint { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public bool? IsEmployeed { get; set; }

    public virtual Jobtitle? Jobtitle { get; set; }

    public virtual ICollection<OrderPlacement> OrderPlacements { get; set; } = new List<OrderPlacement>();

    public virtual PickupPoint? PickupPointNavigation { get; set; }
}
