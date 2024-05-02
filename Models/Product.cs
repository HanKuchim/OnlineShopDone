using System;
using System.Collections.Generic;

namespace OnlineShop.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Rating { get; set; }

    public decimal? Price { get; set; }

    public int? VendorId { get; set; }

    public virtual ICollection<ProductInOrder> ProductInOrders { get; set; } = new List<ProductInOrder>();

    public virtual Vendor? Vendor { get; set; }
}
