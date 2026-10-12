using SmartEnergy.Application.DTOs.Meters;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public interface IMeterService
{
    Task<MeterDto?> GetByIdAsync(Guid id);

    Task<MeterDto?> GetByMeterNumberAsync(
        string meterNumber);

    Task<IReadOnlyList<MeterDto>> GetAllAsync();

    Task<Meter> CreateAsync(
        CreateMeterRequest request);
}