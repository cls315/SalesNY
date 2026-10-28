using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SalesNY.Server.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Tienda> Tiendas { get; set; }

    public virtual DbSet<TipoVentum> TipoVenta { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=aws-0-us-east-1.pooler.supabase.com;Port=6543;Database=sales_ny;Username=postgres.vlczdxdsizfdloinnjov;Password=N0P454R4NUAITERCERO;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("clientes_pkey");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.HomeStoreId, "ix_clientes_store");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_id");
            entity.Property(e => e.BirthYear).HasColumnName("birth_year");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(100)
                .HasColumnName("customer_email");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerSince).HasColumnName("customer_since");
            entity.Property(e => e.Gender)
                .HasMaxLength(15)
                .HasColumnName("gender");
            entity.Property(e => e.HomeStoreId).HasColumnName("home_store_id");
            entity.Property(e => e.LoyaltyCardNumber)
                .HasMaxLength(20)
                .HasColumnName("loyalty_card_number");

            entity.HasOne(d => d.HomeStore).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.HomeStoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clientes_home_store_id_fkey");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("empleados_pkey");

            entity.ToTable("empleados");

            entity.Property(e => e.EmpleadoId)
                .ValueGeneratedNever()
                .HasColumnName("empleado_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Location)
                .HasMaxLength(10)
                .HasColumnName("location");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .HasColumnName("position");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.ProductGroup)
                .HasMaxLength(50)
                .HasColumnName("product_group");
        });

        modelBuilder.Entity<Tienda>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("tiendas_pkey");

            entity.ToTable("tiendas");

            entity.Property(e => e.StoreId)
                .ValueGeneratedNever()
                .HasColumnName("store_id");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.Neighborhood)
                .HasMaxLength(50)
                .HasColumnName("neighborhood");
            entity.Property(e => e.StoreAddress)
                .HasMaxLength(100)
                .HasColumnName("store_address");
            entity.Property(e => e.StoreCity)
                .HasMaxLength(50)
                .HasColumnName("store_city");
            entity.Property(e => e.StoreLatitude)
                .HasPrecision(10, 6)
                .HasColumnName("store_latitude");
            entity.Property(e => e.StoreLongitude)
                .HasPrecision(10, 6)
                .HasColumnName("store_longitude");
            entity.Property(e => e.StorePostalCode)
                .HasMaxLength(10)
                .HasColumnName("store_postal_code");
            entity.Property(e => e.StoreSquareFeet).HasColumnName("store_square_feet");
            entity.Property(e => e.StoreStateProvince)
                .HasMaxLength(5)
                .HasColumnName("store_state_province");
            entity.Property(e => e.StoreType)
                .HasMaxLength(20)
                .HasColumnName("store_type");

            entity.HasOne(d => d.Manager).WithMany(p => p.Tienda)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("tiendas_manager_id_fkey");
        });

        modelBuilder.Entity<TipoVentum>(entity =>
        {
            entity.HasKey(e => e.TipoVentaId).HasName("tipo_venta_pkey");

            entity.ToTable("tipo_venta");

            entity.Property(e => e.TipoVentaId)
                .ValueGeneratedNever()
                .HasColumnName("tipo_venta_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("ventas_pkey");

            entity.ToTable("ventas");

            entity.HasIndex(e => e.CustomerId, "ix_ventas_customer");

            entity.HasIndex(e => e.TransactionDate, "ix_ventas_fecha");

            entity.HasIndex(e => e.ProductId, "ix_ventas_product");

            entity.HasIndex(e => e.StaffId, "ix_ventas_staff");

            entity.HasIndex(e => e.StoreId, "ix_ventas_store");

            entity.HasIndex(e => e.TipoVentaId, "ix_ventas_tipo");

            entity.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("transaction_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Promo)
                .HasMaxLength(2)
                .HasColumnName("promo");
            entity.Property(e => e.QuantitySold).HasColumnName("quantity_sold");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.TipoVentaId).HasColumnName("tipo_venta_id");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Customer).WithMany(p => p.Venta)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_customer_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Venta)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_product_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.Venta)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_staff_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.Venta)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_store_id_fkey");

            entity.HasOne(d => d.TipoVenta).WithMany(p => p.Venta)
                .HasForeignKey(d => d.TipoVentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_tipo_venta_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
