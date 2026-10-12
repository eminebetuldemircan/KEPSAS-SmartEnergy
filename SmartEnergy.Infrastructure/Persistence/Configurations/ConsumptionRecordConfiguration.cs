using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Configurations;

public class ConsumptionRecordConfiguration
    : IEntityTypeConfiguration<ConsumptionRecord>
{
    public void Configure(EntityTypeBuilder<ConsumptionRecord> builder)
    {
        builder.ToTable("ConsumptionRecords");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ConsumptionKwh)
            .HasPrecision(18, 3);

        builder.Property(x => x.MeterIndex)
            .HasPrecision(18, 3);

        builder.HasOne(x => x.Meter)
            .WithMany(x => x.ConsumptionRecords)
            .HasForeignKey(x => x.MeterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.MeterId,
            x.ReadingDate
        });
    }
}