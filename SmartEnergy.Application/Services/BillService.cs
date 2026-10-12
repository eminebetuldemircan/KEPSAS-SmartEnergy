using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.DTOs.Bills;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public class BillService : IBillService
{
    private readonly IBillRepository _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BillService(
        IBillRepository billRepository,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Bill?> GetByIdAsync(Guid id)
    {
        return await _billRepository.GetByIdAsync(id);
    }

    public async Task<Bill?> GetByBillNumberAsync(
        string billNumber)
    {
        return await _billRepository
            .GetByBillNumberAsync(billNumber);
    }

    public async Task<IReadOnlyList<Bill>> GetAllAsync()
    {
        return await _billRepository.GetAllAsync();
    }

    public async Task<IReadOnlyList<Bill>>
        GetBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await _billRepository
            .GetBySubscriptionIdAsync(subscriptionId);
    }

    public async Task<Bill> CreateAsync(
        CreateBillRequest request)
    {
        var existingBill =
            await _billRepository
                .GetByBillNumberAsync(request.BillNumber);

        if (existingBill is not null)
        {
            throw new InvalidOperationException(
                "Bu fatura numarası zaten kayıtlı.");
        }

        if (request.CurrentIndex < request.PreviousIndex)
        {
            throw new InvalidOperationException(
                "Güncel sayaç endeksi önceki endeksten küçük olamaz.");
        }

        var bill = new Bill(
            request.SubscriptionId,
            request.BillNumber,
            request.BillingPeriodStart,
            request.BillingPeriodEnd,
            request.IssueDate,
            request.DueDate,
            request.PreviousIndex,
            request.CurrentIndex,
            request.EnergyAmount,
            request.DistributionAmount,
            request.TaxAmount);

        await _billRepository.AddAsync(bill);

        await _unitOfWork.SaveChangesAsync();

        return bill;
    }
}