using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Store.Customer.API.Domain.Data.Entitys;

namespace Store.Customer.API.Domain.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<AddressEntity>
    {
        public void Configure(EntityTypeBuilder<AddressEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.StreetAddress).IsRequired().HasColumnType("varchar(200)");
            builder.Property(c => c.BuildingNumber).IsRequired().HasColumnType("varchar(50)");
            builder.Property(c => c.SecondaryAddress).IsRequired().HasColumnType("varchar(250)");
            builder.Property(c => c.Neighborhood).IsRequired().HasColumnType("varchar(100)");
            builder.Property(c => c.ZipCode).IsRequired().HasColumnType("varchar(20)");
            builder.Property(c => c.City).IsRequired().HasColumnType("varchar(100)");
            builder.Property(c => c.State).IsRequired().HasColumnType("varchar(250)");


            builder.ToTable("Addresses");
        }
    }
}
