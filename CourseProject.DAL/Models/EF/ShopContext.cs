using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.DAL.Models.EF;

public partial class ShopContext : DbContext
{
    public ShopContext()
    {
    }

    public ShopContext(DbContextOptions<ShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Dasha\\OneDrive\\Рабочий стол\\Сourse work\\CourseProject\\CourseProject.DAL\\Database\\TestDB.mdf;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07437B0E8E");

            entity.ToTable("Category");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC078691B296");

            entity.ToTable("Customer");

            entity.Property(e => e.Addres)
                .HasMaxLength(80)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.CustomerSurname)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<CustomerOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07219FE992");

            entity.ToTable("CustomerOrder");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerO__Custo__38996AB5");

            entity.HasOne(d => d.Seller).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerO__Selle__37A5467C");

            entity.HasOne(d => d.Status).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerO__Statu__398D8EEE");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC07A5B62FBD");

            entity.ToTable("Manufacturer");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC077F8F002D");

            entity.Property(e => e.TotalAmount).HasDefaultValueSql("((1))");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.CustomerOrder).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CustomerOrderId)
                .HasConstraintName("FK__OrderDeta__Custo__4BAC3F29");

            entity.HasOne(d => d.Transport).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.TransportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Trans__4CA06362");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seller__3214EC07B457A6B8");

            entity.ToTable("Seller");

            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.SellerName)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.SellerSurname)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StatusOr__3214EC073F7B2EF4");

            entity.ToTable("StatusOrder");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transpor__3214EC07E76A6BC7");

            entity.ToTable("Transport");

            entity.Property(e => e.Amount).HasDefaultValueSql("((1))");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Category).WithMany(p => p.Transports)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transport__Categ__4316F928");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Transports)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transport__Manuf__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
