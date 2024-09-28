using Core.Extensions;
using Core.Domain.Repository.Data;
using Core.Mediator;
using Core.Message;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Store.Orders.Domain.Data.Entitys.Orders;
using Store.Orders.Domain.Data.Entitys.Vouchers;

namespace Store.Orders.Infra.Data.Contexts
{
    public class OrdersDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;


        public OrdersDbContext(DbContextOptions options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<VoucherEntity> Vouchers { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<OrderItemEntity> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("DateAdded") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DateAdded").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    //NAO ATUALIZAR 
                    entry.Property("DateAdded").IsModified = false;
                    entry.Property("Code").IsModified = false;
                }
            }

            bool success = await base.SaveChangesAsync() > 0;

            if (success)
            {
                await _mediatorHandler.PublishEvents(this);
            }

            return success;
        }
    }
}
