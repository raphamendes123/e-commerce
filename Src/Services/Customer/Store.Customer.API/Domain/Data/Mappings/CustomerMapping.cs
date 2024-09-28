using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Store.Customer.API.Domain.Data.Entitys;
using Core.Domain.Repository.DomainObjects;

namespace Store.Customer.API.Domain.Data.Mappings
{
    public class CustomerMapping : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).IsRequired().HasColumnType("varchar(250)");

            //OwnsOne : CPF Pertence o CLIENTE
            builder.OwnsOne(c => c.Cpf, tf =>
            {
                tf.Property(c => c.Number)
                .IsRequired()
                .HasMaxLength(Cpf.CpfMaxLength)
                .HasColumnName("Cpf")
                .HasColumnType(typeName: $"varchar({Cpf.CpfMaxLength})");
            });

            //OwnsOne : EMAIL Pertence o CLIENTE
            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.Address)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType(typeName: $"varchar({Email.EmailMaxLength})");
            });

            //1 : 1 => CLIENTE : ENDERECO 
            builder.HasOne(c => c.Address)
                .WithOne(c => c.Customer);

            builder.ToTable("Customers");
        }
    }
}
