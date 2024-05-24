using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

namespace WebApi.Data;

public partial class WebApiDBContext : DbContext
{


    private readonly IConfiguration Configuration;


    public WebApiDBContext(DbContextOptions<WebApiDBContext> options, IConfiguration configuration)
        : base(options)
    {
        Configuration = configuration;
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {

        IConfiguration configuration = Configuration;
        services.AddSingleton(configuration);
        services.AddDbContext<WebApiDBContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_time");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_time");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.TotalItem).HasColumnName("total_item");
            entity.Property(e => e.TotalOrderPrice).HasColumnName("total_order_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_time");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_time");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(255)
                .HasColumnName("name_product");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Username, "user_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_time");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_time");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
