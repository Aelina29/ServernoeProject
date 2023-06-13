using System;
using System.Collections.Generic;

namespace Floristic;

public partial class Order
{
    public int Id { get; set; }

    public int? BouquetId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string? Address { get; set; }

    public int? FloristId { get; set; }

    public virtual Bouquet? Bouquet { get; set; }

    public virtual Florist? Florist { get; set; }
}
