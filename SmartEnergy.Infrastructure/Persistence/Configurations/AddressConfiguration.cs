using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.District)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Neighborhood)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Street)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.BuildingNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.ApartmentNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(10);

        builder.Property(x => x.FullAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}