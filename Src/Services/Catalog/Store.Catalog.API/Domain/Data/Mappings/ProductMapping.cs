using Store.Catalog.API.Domain.Data.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Catalog.API.Domain.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).IsRequired().HasColumnType("varchar(250)");
            builder.Property(c => c.Description).IsRequired().HasColumnType("varchar(500)");
            builder.Property(c => c.Image).IsRequired().HasColumnType("varchar(250)");

            builder.ToTable("Products");
        }
    }
}
