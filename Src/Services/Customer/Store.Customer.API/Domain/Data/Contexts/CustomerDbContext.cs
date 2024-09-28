using Core.Domain.Repository.Data;
using Core.Extensions;
using Core.Mediator;
using Core.Message;
using Store.Customer.API.Domain.Data.Entitys;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Store.Customer.API.Domain.Data.Contexts
{
    public class CustomerDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;
        public CustomerDbContext(DbContextOptions options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<AddressEntity> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            //desabilitada delete em cascata DeleteBehavior
            foreach (var relationShip in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationShip.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            bool success = await base.SaveChangesAsync() > 0;

            if (success)
            {
                await _mediatorHandler.PublishEvents(this);
            }

            return success;
        }
    }
}
