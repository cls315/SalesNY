using System;
using System.Collections.Generic;

namespace SalesNY.Server.Models;

public partial class Cliente
{
    public int CustomerId { get; set; }

    public int HomeStoreId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerEmail { get; set; }

    public DateOnly? CustomerSince { get; set; }

    public string? LoyaltyCardNumber { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string? Gender { get; set; }

    public int? BirthYear { get; set; }

    public virtual Tienda HomeStore { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
