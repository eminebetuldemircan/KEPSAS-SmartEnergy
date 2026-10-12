using SmartEnergy.Application.DTOs.ConsumptionRecords;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public interface IConsumptionRecordService
{
    Task<ConsumptionRecord?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<ConsumptionRecordListItemDto>>
        GetAllAsync();

    Task<IReadOnlyList<ConsumptionRecordListItemDto>>
    GetByMeterIdAsync(Guid meterId);

    Task<IReadOnlyList<ConsumptionRecord>>
        GetByDateRangeAsync(
            Guid meterId,
            DateTime startDate,
            DateTime endDate);

    Task<ConsumptionRecord> CreateAsync(
        CreateConsumptionRecordRequest request);
}