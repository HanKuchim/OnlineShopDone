using System;
using System.Collections.Generic;

namespace OnlineShop.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public DateTime? OrderDatetime { get; set; }

    public int? PickupPointId { get; set; }

    public decimal? Price { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<OrderPlacement> OrderPlacements { get; set; } = new List<OrderPlacement>();

    public virtual PickupPoint? PickupPoint { get; set; }

    public virtual ICollection<ProductInOrder> ProductInOrders { get; set; } = new List<ProductInOrder>();
}
