namespace SmartEnergy.Application.DTOs.Addresses;

public class CreateAddressRequest
{
    public Guid UserId { get; set; }

    public string City { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string Neighborhood { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string BuildingNumber { get; set; } = string.Empty;

    public string ApartmentNumber { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;
}