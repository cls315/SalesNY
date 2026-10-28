using System;
using System.Collections.Generic;

namespace SalesNY.Server.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Position { get; set; }

    public DateOnly? StartDate { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Tienda> Tienda { get; set; } = new List<Tienda>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
