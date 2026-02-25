using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTeeth.Persistence.Configurations;

internal class DentalOfficeConfig :IEntityTypeConfiguration<DentalOffice>
{
    public void Configure(EntityTypeBuilder<DentalOffice> builder)
    {
        builder.Property(prop => prop.Name)
            .HasMaxLength(150)
            .IsRequired();
        builder.Property(prop => prop.Address)
            .HasConversion(a => a.ToString(),
                a => Address.FromString(a)
            )
            .IsRequired()
            .HasMaxLength(200);

    }
}