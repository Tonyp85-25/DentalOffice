using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTeeth.Persistence.Configurations;

public class DentistConfig:IEntityTypeConfiguration<Dentist>
{
    public void Configure(EntityTypeBuilder<Dentist> builder)
    {
       builder.Property(prop=>prop.FirstName)
           .HasMaxLength(30)
           .IsRequired();

       builder.Property(prop=>prop.LastName)
           .HasMaxLength(30)
           .IsRequired();

       builder.Property(prop=>prop.Email)
           .HasConversion(
               e=>e.Value,
               v=>new Email(v)
               )
           .IsRequired();

       builder.HasMany(prop => prop.DentalOffices).WithMany("Dentists");

    }
}
