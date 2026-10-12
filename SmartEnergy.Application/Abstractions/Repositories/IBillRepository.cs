using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Abstractions.Repositories;

public interface IBillRepository
{
    Task<Bill?> GetByIdAsync(Guid id);

    Task<Bill?> GetByBillNumberAsync(
        string billNumber);

    Task<IReadOnlyList<Bill>> GetAllAsync();

    Task<IReadOnlyList<Bill>> GetBySubscriptionIdAsync(
        Guid subscriptionId);

    Task AddAsync(Bill bill);

    void Update(Bill bill);

    void Remove(Bill bill);
}