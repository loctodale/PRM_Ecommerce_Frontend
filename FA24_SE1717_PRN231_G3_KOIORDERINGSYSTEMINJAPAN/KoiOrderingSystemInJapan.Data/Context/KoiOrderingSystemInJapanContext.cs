using KoiOrderingSystemInJapan.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace KoiOrderingSystemInJapan.Data.Context;

public partial class KoiOrderingSystemInJapanContext : DbContext
{
    public KoiOrderingSystemInJapanContext()
    {
    }

    public KoiOrderingSystemInJapanContext(DbContextOptions<KoiOrderingSystemInJapanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<BookingRequest> BookingRequests { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<DeliveryDetail> DeliveryDetails { get; set; }

    public virtual DbSet<Farm> Farms { get; set; }

    public virtual DbSet<FarmCategory> FarmCategories { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<KoiFish> KoiFishes { get; set; }

    public virtual DbSet<KoiOrder> KoiOrders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Travel> Travels { get; set; }

    public virtual DbSet<TravelFarm> TravelFarms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
        });

        modelBuilder.Entity<ServiceXBookingRequest>(entity =>
        {
            entity.HasKey(e => new { e.ServiceId, e.BookingRequestId });
            entity.ToTable("ServiceXBookingRequest");
        });

        modelBuilder.Entity<BookingRequest>(entity =>
        {
            var converterBookingRequestStatus = new EnumToStringConverter<ConstEnum.BookingRequestStatus>();

            entity.HasKey(e => e.Id);

            entity.ToTable("BookingRequest");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");

            entity.Property(x => x.Status)
                .HasConversion(converterBookingRequestStatus);

            entity.HasOne(d => d.Customer).WithMany(p => p.BookingRequests)
                .HasForeignKey(d => d.CustomerId)
                ;

            entity.HasOne(d => d.Travel).WithMany(p => p.BookingRequests)
                .HasForeignKey(d => d.TravelId)
                ;

            entity.HasMany(e => e.ServiceOrders)
                .WithOne(e => e.BookingRequest)
                .HasForeignKey(e => e.BookingRequestId)
                ;

            entity.HasMany(e => e.ServiceXBookingRequest)
                .WithOne(e => e.BookingRequest)
                .HasForeignKey(e => e.BookingRequestId)
                ;

            entity.HasOne(d => d.Sale).WithOne(p => p.BookingRequest)
                .HasForeignKey<Sale>(d => d.BookingRequestId)
                ;
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Delivery");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.DeliveryStaff).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.DeliveryStaffId)
                ;

            entity.HasOne(d => d.KoiOrder).WithOne(p => p.Deliveries)
                .HasForeignKey<Delivery>(d => d.KoiOrderId)
                ;
        });

        modelBuilder.Entity<DeliveryDetail>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("DeliveryDetail");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");

            entity.HasOne(d => d.Delivery).WithMany(p => p.DeliveryDetails)
                .HasForeignKey(d => d.DeliveryId)
                ;
        });

        modelBuilder.Entity<Farm>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Farm");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
        });

        modelBuilder.Entity<FarmCategory>(entity =>
        {
            entity.HasKey(e => new { e.FarmId, e.CategoryId });

            entity.ToTable("FarmCategory");

            entity.HasOne(d => d.Category).WithMany(p => p.FarmCategories)
                .HasForeignKey(d => d.CategoryId)
                ;

            entity.HasOne(d => d.Farm).WithMany(p => p.FarmCategories)
                .HasForeignKey(d => d.FarmId)
                ;
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Invoice");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<KoiFish>(entity =>
        {
            var converterGender = new EnumToStringConverter<ConstEnum.Gender>();

            entity.HasKey(e => e.Id);

            entity.ToTable("KoiFish");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.Property(x => x.Gender)
                .HasConversion(converterGender);

            entity.HasOne(d => d.Category).WithMany(p => p.KoiFishes)
                .HasForeignKey(d => d.CategoryId)
                ;

            entity.HasOne(d => d.Size).WithOne(p => p.KoiFish)
                .HasForeignKey<Size>(d => d.KoiFishId)
                ;
        });

        modelBuilder.Entity<KoiOrder>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("KoiOrder");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.KoiOrders)
                .HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Invoice).WithOne(p => p.KoiOrder)
                .HasForeignKey<KoiOrder>(d => d.InvoiceId);
        });
         
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.KoiFish).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.KoiFishId)
                ;

            entity.HasOne(d => d.KoiOrder).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.KoiOrderId)
                ;
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            var converterStatusSale = new EnumToStringConverter<ConstEnum.StatusSale>();

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.Property(x => x.Status)
                .HasConversion(converterStatusSale);

            entity.HasOne(d => d.SaleStaff).WithMany(p => p.SaleSaleStaffs)
                .HasForeignKey(d => d.SaleStaffId)
                ;
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Service");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasMany(e => e.ServiceXBookingRequest)
                .WithOne(e => e.Service)
                .HasForeignKey(e => e.ServiceId)
                ;
        });

        modelBuilder.Entity<ServiceOrder>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("ServiceOrder");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.BookingRequest).WithMany(p => p.ServiceOrders)
                .HasForeignKey(d => d.BookingRequestId)
                ;

            entity.HasOne(d => d.Invoice).WithOne(p => p.ServiceOrder)
                .HasForeignKey<ServiceOrder>(d => d.InvoiceId)
                ;
        });

        modelBuilder.Entity<Travel>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Travel");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TravelFarm>(entity =>
        {
            entity.HasKey(e => new { e.TravelId, e.FarmId });

            entity.ToTable("TravelFarm");

            entity.HasOne(d => d.Farm).WithMany(p => p.TravelFarms)
                .HasForeignKey(d => d.FarmId)
                ;

            entity.HasOne(d => d.Travel).WithMany(p => p.TravelFarms)
                .HasForeignKey(d => d.TravelId)
                ;
        });

        modelBuilder.Entity<User>(entity =>
        {
            var converterRole = new EnumToStringConverter<ConstEnum.Role>();
            var converterGender = new EnumToStringConverter<ConstEnum.Gender>();

            entity.HasKey(e => e.Id);

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
            entity.Property(x => x.Role)
                .HasConversion(converterRole);
            entity.Property(x => x.Gender)
                .HasConversion(converterGender);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Size");

            entity.Property(e => e.SizeInCm).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SizeInGram).HasColumnType("decimal(10, 2)");


            entity.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}