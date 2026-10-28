using System;
using System.Collections.Generic;

namespace SalesNY.Server.Models;

public partial class TipoVentum
{
    public int TipoVentaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
