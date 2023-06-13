using System;
using System.Collections.Generic;

namespace Floristic;

public partial class Florist
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? ShortName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
