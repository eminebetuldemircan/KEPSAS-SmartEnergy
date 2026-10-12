using SmartEnergy.Domain.Common;

namespace SmartEnergy.Domain.Entities;

public class Address : BaseEntity
{
    public Guid UserId { get; private set; }

    public string City { get; private set; } = string.Empty;

    public string District { get; private set; } = string.Empty;

    public string Neighborhood { get; private set; } = string.Empty;

    public string Street { get; private set; } = string.Empty;

    public string BuildingNumber { get; private set; } = string.Empty;

    public string ApartmentNumber { get; private set; } = string.Empty;

    public string PostalCode { get; private set; } = string.Empty;

    public string FullAddress { get; private set; } = string.Empty;

    public User User { get; private set; } = null!;

    private Address()
    {
    }

    public Address(
        Guid userId,
        string city,
        string district,
        string neighborhood,
        string street,
        string buildingNumber,
        string apartmentNumber,
        string postalCode)
    {
        UserId = userId;
        City = city;
        District = district;
        Neighborhood = neighborhood;
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        PostalCode = postalCode;

        UpdateFullAddress();
    }

    public void UpdateAddress(
        string city,
        string district,
        string neighborhood,
        string street,
        string buildingNumber,
        string apartmentNumber,
        string postalCode)
    {
        City = city;
        District = district;
        Neighborhood = neighborhood;
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        PostalCode = postalCode;

        UpdateFullAddress();
        SetUpdatedAt();
    }

    private void UpdateFullAddress()
    {
        FullAddress =
            $"{Neighborhood}, {Street}, No: {BuildingNumber}, " +
            $"Daire: {ApartmentNumber}, {District}/{City} {PostalCode}";
    }
}