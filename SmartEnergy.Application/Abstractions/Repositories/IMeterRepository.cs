using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Abstractions.Repositories;

public interface IMeterRepository
{
    Task<Meter?> GetByIdAsync(Guid id);

    Task<Meter?> GetByMeterNumberAsync(string meterNumber);

    Task<IReadOnlyList<Meter>> GetAllAsync();

    Task AddAsync(Meter meter);

    void Update(Meter meter);

    void Remove(Meter meter);
}