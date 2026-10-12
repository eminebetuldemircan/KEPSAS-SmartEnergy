using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Configurations;

public class SubscriptionConfiguration
    : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SubscriptionNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.SubscriptionNumber)
            .IsUnique();

        builder.Property(x => x.ContractNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.ContractNumber)
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Meter)
            .WithOne(x => x.Subscription)
            .HasForeignKey<Subscription>(x => x.MeterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}