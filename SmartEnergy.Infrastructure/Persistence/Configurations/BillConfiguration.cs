using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BillNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.BillNumber)
            .IsUnique();

        builder.Property(x => x.PreviousIndex)
            .HasPrecision(18, 3);

        builder.Property(x => x.CurrentIndex)
            .HasPrecision(18, 3);

        builder.Property(x => x.ConsumptionKwh)
            .HasPrecision(18, 3);

        builder.Property(x => x.EnergyAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.DistributionAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.Subscription)
            .WithMany(x => x.Bills)
            .HasForeignKey(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}