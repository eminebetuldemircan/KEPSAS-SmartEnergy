using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Configurations;

public class MeterConfiguration : IEntityTypeConfiguration<Meter>
{
    public void Configure(EntityTypeBuilder<Meter> builder)
    {
        builder.ToTable("Meters");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MeterNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.MeterNumber)
            .IsUnique();

        builder.Property(x => x.SerialNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastIndex)
            .HasPrecision(18, 3);
    }
}