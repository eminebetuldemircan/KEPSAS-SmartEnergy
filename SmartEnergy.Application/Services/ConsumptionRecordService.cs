using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.DTOs.ConsumptionRecords;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public class ConsumptionRecordService
    : IConsumptionRecordService
{
    private readonly IConsumptionRecordRepository _repository;
    private readonly IMeterRepository _meterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConsumptionRecordService(
        IConsumptionRecordRepository repository,
        IMeterRepository meterRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _meterRepository = meterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ConsumptionRecord?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IReadOnlyList<ConsumptionRecordListItemDto>>
        GetAllAsync()
    {
        var records = await _repository.GetAllAsync();

        return records.Select(x => new ConsumptionRecordListItemDto
        {
            Id = x.Id,
            MeterId = x.MeterId,
            MeterNumber = x.Meter?.MeterNumber ?? "Bilinmiyor",
            ReadingDate = x.ReadingDate,
            ConsumptionKwh = x.ConsumptionKwh,
            MeterIndex = x.MeterIndex,
            IsEstimated = x.IsEstimated
        }).ToList();
    }

    public async Task<IReadOnlyList<ConsumptionRecordListItemDto>>
    GetByMeterIdAsync(Guid meterId)
    {
        var records = await _repository.GetByMeterIdAsync(meterId);

        return records.Select(x => new ConsumptionRecordListItemDto
        {
            Id = x.Id,
            MeterId = x.MeterId,
            MeterNumber = x.Meter?.MeterNumber ?? "Bilinmiyor",
            ReadingDate = x.ReadingDate,
            ConsumptionKwh = x.ConsumptionKwh,
            MeterIndex = x.MeterIndex,
            IsEstimated = x.IsEstimated
        }).ToList();
    }

    public async Task<IReadOnlyList<ConsumptionRecord>>
        GetByDateRangeAsync(
            Guid meterId,
            DateTime startDate,
            DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new InvalidOperationException(
                "Başlangıç tarihi bitiş tarihinden büyük olamaz.");
        }

        return await _repository.GetByDateRangeAsync(
            meterId,
            startDate,
            endDate);
    }

    public async Task<ConsumptionRecord> CreateAsync(
        CreateConsumptionRecordRequest request)
    {
        var meter = await _meterRepository
            .GetByIdAsync(request.MeterId);

        if (meter is null)
        {
            throw new InvalidOperationException(
                "Sayaç bulunamadı.");
        }

        if (request.MeterIndex < 0)
        {
            throw new InvalidOperationException(
                "Sayaç endeksi negatif olamaz.");
        }

        if (request.MeterIndex < meter.LastIndex)
        {
            throw new InvalidOperationException(
                "Yeni sayaç endeksi mevcut endeksten küçük olamaz.");
        }

        var consumptionKwh =
            request.MeterIndex - meter.LastIndex;

        var record = new ConsumptionRecord(
            request.MeterId,
            request.ReadingDate,
            consumptionKwh,
            request.MeterIndex,
            request.IsEstimated);

        await _repository.AddAsync(record);

        meter.UpdateReading(
            request.MeterIndex,
            request.ReadingDate);

        await _unitOfWork.SaveChangesAsync();

        return record;
    }
}