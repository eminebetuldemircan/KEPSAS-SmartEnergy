using SmartEnergy.Application.DTOs.Bills;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public interface IBillService
{
    Task<Bill?> GetByIdAsync(Guid id);

    Task<Bill?> GetByBillNumberAsync(
        string billNumber);

    Task<IReadOnlyList<Bill>> GetAllAsync();

    Task<IReadOnlyList<Bill>> GetBySubscriptionIdAsync(
        Guid subscriptionId);

    Task<Bill> CreateAsync(
        CreateBillRequest request);
}