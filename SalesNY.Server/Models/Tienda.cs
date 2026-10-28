using System;
using System.Collections.Generic;

namespace SalesNY.Server.Models;

public partial class Tienda
{
    public int StoreId { get; set; }

    public string? StoreType { get; set; }

    public int? StoreSquareFeet { get; set; }

    public string? StoreAddress { get; set; }

    public string? StoreCity { get; set; }

    public string? StoreStateProvince { get; set; }

    public string? StorePostalCode { get; set; }

    public decimal? StoreLongitude { get; set; }

    public decimal? StoreLatitude { get; set; }

    public int? ManagerId { get; set; }

    public string? Neighborhood { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual Empleado? Manager { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
