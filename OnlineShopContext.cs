using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop;

public partial class OnlineShopContext : DbContext
{
    public OnlineShopContext()
    {
    }

    public OnlineShopContext(DbContextOptions<OnlineShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Jobtitle> Jobtitles { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderPlacement> OrderPlacements { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductInOrder> ProductInOrders { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OnlineShop;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_CI_AS");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Jobtitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Job_Title");

            entity.ToTable("Jobtitle");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("clientId");
            entity.Property(e => e.OrderDatetime).HasColumnType("datetime");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_order_clients");

            entity.HasOne(d => d.PickupPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickupPointId)
                .HasConstraintName("FK_order_PickUpPoint");
        });

        modelBuilder.Entity<OrderPlacement>(entity =>
        {
            entity.ToTable("OrderPlacement");

            entity.Property(e => e.OrderPlacementDate).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPlacements)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderPlacement_order");

            entity.HasOne(d => d.Worker).WithMany(p => p.OrderPlacements)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("FK_OrderPlacement_Workers");
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.ToTable("PickupPoint");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Products)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK_product_Vendor");
        });

        modelBuilder.Entity<ProductInOrder>(entity =>
        {
            entity.ToTable("ProductInOrder", tb => tb.HasTrigger("CalculateOrderPrice"));

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Order).WithMany(p => p.ProductInOrders)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("FK_ProductInOrder_order");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductInOrders)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("FK_ProductInOrder_product");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.ToTable("vendor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsEmployeed).HasDefaultValue(true);
            entity.Property(e => e.Jobtitleid).HasColumnName("jobtitleid");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("money");

            entity.HasOne(d => d.Jobtitle).WithMany(p => p.Workers)
                .HasForeignKey(d => d.Jobtitleid)
                .HasConstraintName("FK_Workers_jobtitle");

            entity.HasOne(d => d.PickupPointNavigation).WithMany(p => p.Workers)
                .HasForeignKey(d => d.PickupPoint)
                .HasConstraintName("FK_Workers_PickupPoint");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
