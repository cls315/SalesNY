using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesNY.Server.Models;

namespace SalesNY.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSales(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] int? productId,
            [FromQuery] int? storeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var baseQuery = _context.Ventas
                .AsNoTracking()
                .OrderByDescending(v => v.TransactionId)
                .Take(1000);

                var query = baseQuery.AsQueryable();

                if (fechaInicio.HasValue)
                {
                    var dInicio = DateOnly.FromDateTime(fechaInicio.Value);
                    query = query.Where(v => v.TransactionDate >= dInicio);
                }

                if (fechaFin.HasValue)
                {
                    var dFin = DateOnly.FromDateTime(fechaFin.Value);
                    query = query.Where(v => v.TransactionDate <= dFin);
                }

                if (productId.HasValue)
                    query = query.Where(v => v.ProductId == productId.Value);

                if (storeId.HasValue)
                    query = query.Where(v => v.StoreId == storeId.Value);

                var sales = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(v => new
                    {
                        v.TransactionId,
                        v.TransactionDate,
                        v.QuantitySold,
                        v.UnitPrice,
                        Total = v.QuantitySold * v.UnitPrice,
                        v.Promo,
                        ProductGroup = v.Product != null ? v.Product.ProductGroup : "N/A",
                        CustomerName = v.Customer != null ? v.Customer.CustomerName : "N/A",
                        StaffName = v.Staff != null ? $"{v.Staff.FirstName} {v.Staff.LastName}" : "N/A",
                        StoreCity = v.Store != null ? v.Store.StoreCity : "N/A",
                        TipoVenta = v.TipoVenta != null ? v.TipoVenta.Descripcion : "N/A"
                    })
                    .ToListAsync();

                return Ok(new
                {
                    Page = page,
                    PageSize = pageSize,
                    LimitApplied = 1000,
                    Data = sales
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            
        }

        [HttpGet("dashboard/ventas-por-tienda")]
        public async Task<IActionResult> GetVentasPorTienda()
        {
            try
            {
                var result = await _context.Ventas
                    .AsNoTracking()
                    .GroupBy(v => v.Store.StoreCity ?? "Sin Ciudad")
                    .Select(g => new
                    {
                        Tienda = g.Key,
                        TotalVentas = g.Sum(v => (decimal)v.QuantitySold * v.UnitPrice),
                        CantidadTransacciones = g.Count()
                    })
                    .OrderByDescending(x => x.TotalVentas)
                    .Take(10)
                    .ToListAsync();
            
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, inner = ex.InnerException?.Message  });
            }
        }

        [HttpGet("dashboard/top-productos")]
        public async Task<IActionResult> GetTopProductos([FromQuery] int top = 5)
        {
            try
            {
                var result = await _context.Ventas
                    .AsNoTracking()
                    .GroupBy(v => v.Product.ProductGroup ?? "Desconocido")
                    .Select(g => new
                    {
                        Producto = g.Key,
                        TotalRecaudado = g.Sum(v => (decimal)v.QuantitySold * v.UnitPrice),
                        UnidadesVendidas = g.Sum(v => v.QuantitySold)
                    })
                    .OrderByDescending(x => x.TotalRecaudado)
                    .Take(top)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, inner = ex.InnerException?.Message  });
            }
        }

        [HttpGet("dashboard/kpis")]
        public async Task<IActionResult> GetKPIs()
        {
            try
            { 
                var totalClientes = await _context.Clientes.AsNoTracking().CountAsync();
                var totalTiendas = await _context.Tiendas.AsNoTracking().CountAsync();

                return Ok(new
                {
                    TotalFacturado = 124062954,
                    TotalTransacciones = 767394,
                    TotalClientes = totalClientes,
                    TotalTiendas = totalTiendas
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
