using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Abstractions.Repositories;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByIdAsync(Guid id);
    Task<Subscription?> GetBySubscriptionNumberAsync(string subscriptionNumber);
    Task<IReadOnlyList<Subscription>> GetAllAsync();
    Task<IReadOnlyList<Subscription>> GetByUserIdAsync(Guid userId);

    Task AddAsync(Subscription subscription);

    void Update(Subscription subscription);

    void Remove(Subscription subscription);
}