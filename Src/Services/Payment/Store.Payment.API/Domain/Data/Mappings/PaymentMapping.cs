using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Payment.API.Domain.Data.Entitys;

namespace Store.Payment.API.Domain.Data.Mappings
{
    public class PaymentMapping : IEntityTypeConfiguration<PaymentEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Ignore(c => c.CreditCard);

            // 1 : N => Payment : Transaction
            builder.HasMany(c => c.Transactions)
                .WithOne(c => c.Payment)
                .HasForeignKey(c => c.IdPayment);

            builder.ToTable("Payments");
        }
    }
}
