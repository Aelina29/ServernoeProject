using System;
using System.Collections.Generic;

namespace Floristic;

public partial class Bouquet
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
