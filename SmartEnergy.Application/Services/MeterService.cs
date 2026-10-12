using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.DTOs.Meters;
using SmartEnergy.Domain.Entities;
using SmartEnergy.Domain.Enums;

namespace SmartEnergy.Application.Services;

public class MeterService : IMeterService
{
    private readonly IMeterRepository _meterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MeterService(
        IMeterRepository meterRepository,
        IUnitOfWork unitOfWork)
    {
        _meterRepository = meterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MeterDto?> GetByIdAsync(Guid id)
    {
        var meter =
            await _meterRepository.GetByIdAsync(id);

        if (meter is null)
        {
            return null;
        }

        return MapToDto(meter);
    }

    public async Task<MeterDto?> GetByMeterNumberAsync(
        string meterNumber)
    {
        var meter =
            await _meterRepository
                .GetByMeterNumberAsync(meterNumber);

        if (meter is null)
        {
            return null;
        }

        return MapToDto(meter);
    }

    public async Task<IReadOnlyList<MeterDto>>
        GetAllAsync()
    {
        var meters =
            await _meterRepository.GetAllAsync();

        return meters
            .Select(MapToDto)
            .ToList();
    }

    public async Task<Meter> CreateAsync(
        CreateMeterRequest request)
    {
        var existingMeter =
            await _meterRepository
                .GetByMeterNumberAsync(
                    request.MeterNumber);

        if (existingMeter is not null)
        {
            throw new InvalidOperationException(
                "Bu sayaç numarası zaten kayıtlı.");
        }

        var meter = new Meter(
            request.MeterNumber,
            request.SerialNumber,
            (MeterType)request.Type,
            request.InstallationDate);

        await _meterRepository.AddAsync(meter);

        await _unitOfWork.SaveChangesAsync();

        return meter;
    }

    private static MeterDto MapToDto(
        Meter meter)
    {
        return new MeterDto
        {
            Id = meter.Id,
            MeterNumber = meter.MeterNumber,
            SerialNumber = meter.SerialNumber,
            Type = (int)meter.Type,
            Status = (int)meter.Status,
            InstallationDate = meter.InstallationDate,
            LastReadingDate = meter.LastReadingDate,
            LastIndex = meter.LastIndex
        };
    }
}