using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Store.ShopCart.API.Domain.Data.Entitys;

namespace Store.ShopCart.API.Domain.Data.Contexts
{
    public sealed class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.IdCustomer)
                .HasName("IDX_Customer");

            modelBuilder.Entity<Cart>()
                .Ignore(c => c.Voucher)
                .OwnsOne(c => c.Voucher, v =>
                {
                    v.Property(vc => vc.Code)
                        .HasColumnName("Voucher_Code")
                        .HasColumnType("varchar(50)");

                    v.Property(vc => vc.DiscountType);

                    v.Property(vc => vc.Percentage);

                    v.Property(vc => vc.Discount);
                });

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(c => c.IdCart);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }
    }
}