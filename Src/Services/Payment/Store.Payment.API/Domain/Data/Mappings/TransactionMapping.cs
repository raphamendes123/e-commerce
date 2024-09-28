using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Payment.API.Domain.Data.Entitys;

namespace Store.Payment.API.Domain.Data.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.HasKey(c => c.Id);

            // 1 : N => Payment : Transaction
            builder.HasOne(c => c.Payment)
                .WithMany(c => c.Transactions);

            builder.ToTable("Transactions");
        }
    }
}
