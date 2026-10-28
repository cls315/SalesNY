using System;
using System.Collections.Generic;

namespace SalesNY.Server.Models;

public partial class Producto
{
    public int ProductId { get; set; }

    public string ProductGroup { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
