using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models;

public partial class OnlineShopContext : DbContext
{
    public OnlineShopContext()
    {
    }

    public OnlineShopContext(DbContextOptions<OnlineShopContext> options)
        : base(options)
    {
        Database.EnsureCreated();
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

        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, Name = "Иван Иванов" },
            new Client { Id = 2, Name = "Елена Петрова" },
            new Client { Id = 3, Name = "Александр Сидоров" },
            new Client { Id = 4, Name = "Мария Васнецова" },
            new Client { Id = 5, Name = "Дмитрий Смирнов" },
            new Client { Id = 6, Name = "Ольга Кузнецова" },
            new Client { Id = 7, Name = "Сергей Петров" },
            new Client { Id = 8, Name = "Анастасия Иванова" },
            new Client { Id = 9, Name = "Павел Федоров" },
            new Client { Id = 10, Name = "Татьяна Николаева" }
        );

        modelBuilder.Entity<Vendor>().HasData(
        new Vendor { Id = 1, Name = "АО \"Рога и копыта\"" },
        new Vendor { Id = 2, Name = "ООО \"БыстроСуперТовары\"" },
        new Vendor { Id = 3, Name = "ЗАО \"ГиперМаг\"" },
        new Vendor { Id = 4, Name = "ИП \"Скромный Продавец\"" },
        new Vendor { Id = 5, Name = "ТД \"ЭкоПродукты\"" },
        new Vendor { Id = 6, Name = "ООО \"ЛучшийВыбор\"" },
        new Vendor { Id = 7, Name = "ИП \"ДешевлеНигде\"" },
        new Vendor { Id = 8, Name = "АО \"Элитные Товары\"" },
        new Vendor { Id = 9, Name = "ООО \"ГурманТрейд\"" },
        new Vendor { Id = 10, Name = "ТД \"ПроТехнику\"" }
        );

        modelBuilder.Entity<PickupPoint>().HasData(
        new PickupPoint { Id = 1, Adress = "ул. Ленина, д. 1", Rating = 4.2m },
        new PickupPoint { Id = 2, Adress = "пр. Победы, д. 10", Rating = 3.8m },
        new PickupPoint { Id = 3, Adress = "ул. Советская, д. 25", Rating = 4.5m },
        new PickupPoint { Id = 4, Adress = "пер. Цветочный, д. 7", Rating = 3.2m },
        new PickupPoint { Id = 5, Adress = "пл. Революции, д. 15", Rating = 4.7m },
        new PickupPoint { Id = 6, Adress = "пр. Гагарина, д. 30", Rating = 3.9m },
        new PickupPoint { Id = 7, Adress = "ул. Пушкина, д. 5", Rating = 4.1m },
        new PickupPoint { Id = 8, Adress = "бул. Космонавтов, д. 12", Rating = 3.5m },
        new PickupPoint { Id = 9, Adress = "пр. Ленинградский, д. 8", Rating = 4.0m },
        new PickupPoint { Id = 10, Adress = "ул. Маяковского, д. 3", Rating = 3.7m }
        );
        
        modelBuilder.Entity<Jobtitle>().HasData(
            new Jobtitle { Id = 1, Name = "Admin" },
            new Jobtitle { Id = 2, Name = "User" }
        );

        modelBuilder.Entity<Worker>().HasData(
            new Worker { Id = 1, Name = "Иванов Иван Иванович", Salary = 50000, Jobtitleid = 1, PickupPoint = 1, Login = "IvanovIvan", Password = "12345" },
            new Worker { Id = 2, Name = "Петров Петр Петрович", Salary = 48000, Jobtitleid = 1, PickupPoint = 2, Login = "PetrovPetr", Password = "12345" },
            new Worker { Id = 3, Name = "Сидоров Сидор Сидорович", Salary = 55000, Jobtitleid = 1, PickupPoint = 3, Login = "SidorovSidor", Password = "12345" },
            new Worker { Id = 4, Name = "Васнецова Мария Васильевна", Salary = 52000, Jobtitleid = 2, PickupPoint = 4, Login = "VasnetsovaMaria", Password = "12345" },
            new Worker { Id = 5, Name = "Смирнов Дмитрий Александрович", Salary = 53000, Jobtitleid = 2, PickupPoint = 5, Login = "SmirnovDmitriy", Password = "12345" },
            new Worker { Id = 6, Name = "Кузнецова Ольга Дмитриевна", Salary = 49000, Jobtitleid = 2, PickupPoint = 6, Login = "KuznetsovaOlga", Password = "12345" },
            new Worker { Id = 7, Name = "Петров Сергей Петрович", Salary = 51000, Jobtitleid = 2, PickupPoint = 7, Login = "PetrovSergey", Password = "12345" },
            new Worker { Id = 8, Name = "Иванова Анастасия Сергеевна", Salary = 48000, Jobtitleid = 2, PickupPoint = 8, Login = "IvanovaAnastasya", Password = "12345" },
            new Worker { Id = 9, Name = "Федоров Павел Александрович", Salary = 54000, Jobtitleid = 2, PickupPoint = 9, Login = "FedorovPavel", Password = "12345" },
            new Worker { Id = 10, Name = "Николаева Татьяна Павловна", Salary = 50000, Jobtitleid = 2, PickupPoint = 10, Login = "NikolaevaTatyana", Password = "12345" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Телевизор Samsung", Rating = 4.8m, Price = 30000m, VendorId = 1 },
            new Product { Id = 2, Name = "Холодильник LG", Rating = 4.5m, Price = 25000m, VendorId = 2 },
            new Product { Id = 3, Name = "Смартфон Apple iPhone", Rating = 4.9m, Price = 60000m, VendorId = 3 },
            new Product { Id = 4, Name = "Ноутбук HP", Rating = 4.3m, Price = 45000m, VendorId = 4 },
            new Product { Id = 5, Name = "Кофеварка Bosch", Rating = 4.6m, Price = 8000m, VendorId = 5 },
            new Product { Id = 6, Name = "Микроволновка Panasonic", Rating = 4.2m, Price = 12000m, VendorId = 6 },
            new Product { Id = 7, Name = "Пылесос Dyson", Rating = 4.7m, Price = 35000m, VendorId = 7 },
            new Product { Id = 8, Name = "Фен Philips", Rating = 4.1m, Price = 5000m, VendorId = 8 },
            new Product { Id = 9, Name = "Мультиварка Moulinex", Rating = 4.4m, Price = 10000m, VendorId = 9 },
            new Product { Id = 10, Name = "Утюг Tefal", Rating = 4.0m, Price = 7000m, VendorId = 10 }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, PickupPointId = 1, ClientId = 2, Price = 0 , OrderDatetime = DateTime.Now },
            new Order { Id = 2, PickupPointId = 2, ClientId = 5, Price = 0 , OrderDatetime = DateTime.Now },
            new Order { Id = 3, PickupPointId = 3, ClientId = 8, Price = 0 , OrderDatetime = DateTime.Now },
            new Order { Id = 4, PickupPointId = 4, ClientId = 1, Price = 0, OrderDatetime = DateTime.Now },
            new Order { Id = 5, PickupPointId = 5, ClientId = 3, Price = 0, OrderDatetime = DateTime.Now },
            new Order { Id = 6, PickupPointId = 6, ClientId = 6, Price = 0, OrderDatetime = DateTime.Now },
            new Order { Id = 7, PickupPointId = 7, ClientId = 9, Price = 0, OrderDatetime = DateTime.Now },
            new Order { Id = 8, PickupPointId = 8, ClientId = 4, Price = 0, OrderDatetime = DateTime.Now },
            new Order { Id = 9, PickupPointId = 9, ClientId = 7, Price = 0, OrderDatetime = DateTime.Now },
            new Order { Id = 10, PickupPointId = 10, ClientId = 10, Price = 0, OrderDatetime = DateTime.Now }
        );
        modelBuilder.Entity<OrderPlacement>().HasData(
            new OrderPlacement { Id = 1, WorkerId = 1, OrderId = 1, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 2, WorkerId = 2, OrderId = 2, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 3, WorkerId = 3, OrderId = 3, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 4, WorkerId = 4, OrderId = 4, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 5, WorkerId = 5, OrderId = 5, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 6, WorkerId = 6, OrderId = 6, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 7, WorkerId = 7, OrderId = 7, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 8, WorkerId = 8, OrderId = 8, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 9, WorkerId = 9, OrderId = 9, OrderPlacementDate = DateTime.Now },
            new OrderPlacement { Id = 10, WorkerId = 10, OrderId = 10, OrderPlacementDate = DateTime.Now }
        );

        modelBuilder.Entity<ProductInOrder>().HasData(
            new ProductInOrder { Id = 1, Productid = 1, Amount = 2, Orderid = 1 },
            new ProductInOrder { Id = 2, Productid = 3, Amount = 1, Orderid = 2 },
            new ProductInOrder { Id = 3, Productid = 5, Amount = 3, Orderid = 3 },
            new ProductInOrder { Id = 4, Productid = 7, Amount = 2, Orderid = 4 },
            new ProductInOrder { Id = 5, Productid = 9, Amount = 1, Orderid = 5 },
            new ProductInOrder { Id = 6, Productid = 2, Amount = 4, Orderid = 6 },
            new ProductInOrder { Id = 7, Productid = 4, Amount = 1, Orderid = 7 },
            new ProductInOrder { Id = 8, Productid = 6, Amount = 2, Orderid = 8 },
            new ProductInOrder { Id = 9, Productid = 8, Amount = 1, Orderid = 9 },
            new ProductInOrder { Id = 10, Productid = 10, Amount = 3, Orderid = 10 }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
