using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Abstractions.Repositories;

public interface IConsumptionRecordRepository
{
    Task<ConsumptionRecord?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<ConsumptionRecord>> GetAllAsync();

    Task<IReadOnlyList<ConsumptionRecord>> GetByMeterIdAsync(
        Guid meterId);

    Task<IReadOnlyList<ConsumptionRecord>> GetByDateRangeAsync(
        Guid meterId,
        DateTime startDate,
        DateTime endDate);

    Task AddAsync(ConsumptionRecord record);

    void Update(ConsumptionRecord record);

    void Remove(ConsumptionRecord record);
}