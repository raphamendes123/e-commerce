using Core.Security.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace Core.Security.Jwt.EntityFrameworkCore;

public class KeyMaterialMap : IEntityTypeConfiguration<KeyMaterial>
{
    public void Configure(EntityTypeBuilder<KeyMaterial> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Parameters)
            .HasMaxLength(8000)
            .IsRequired();
    }
}