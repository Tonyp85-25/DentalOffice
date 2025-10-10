using CleanTeeth.Domain.Entities;
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
    }
}