using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Orders.Domain.Data.Entitys.Vouchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Orders.Infra.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<VoucherEntity>
    {
        public void Configure(EntityTypeBuilder<VoucherEntity> builder)
        {
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Code)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.ToTable("Vouchers");
        }
    }
}
