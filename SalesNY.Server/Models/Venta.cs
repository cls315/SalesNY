using System;
using System.Collections.Generic;

namespace SalesNY.Server.Models;

public partial class Venta
{
    public int TransactionId { get; set; }

    public DateOnly TransactionDate { get; set; }

    public int StoreId { get; set; }

    public int StaffId { get; set; }

    public int CustomerId { get; set; }

    public int TipoVentaId { get; set; }

    public int ProductId { get; set; }

    public int QuantitySold { get; set; }

    public decimal UnitPrice { get; set; }

    public string? Promo { get; set; }

    public virtual Cliente Customer { get; set; } = null!;

    public virtual Producto Product { get; set; } = null!;

    public virtual Empleado Staff { get; set; } = null!;

    public virtual Tienda Store { get; set; } = null!;

    public virtual TipoVentum TipoVenta { get; set; } = null!;
}
