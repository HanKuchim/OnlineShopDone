using System;
using System.Collections.Generic;

namespace OnlineShop.Models;

public partial class Jobtitle
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
