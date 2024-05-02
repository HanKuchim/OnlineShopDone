using System;
using System.Collections.Generic;

namespace OnlineShop.Models;

public partial class ProductInOrder
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public int? Amount { get; set; }

    public int? Orderid { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
