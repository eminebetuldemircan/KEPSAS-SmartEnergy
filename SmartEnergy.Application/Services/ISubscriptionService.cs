using SmartEnergy.Application.DTOs.Subscriptions;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public interface ISubscriptionService
{
    Task<Subscription?> GetByIdAsync(Guid id);

    Task<Subscription?> GetBySubscriptionNumberAsync(
        string subscriptionNumber);

    Task<IReadOnlyList<Subscription>> GetAllAsync();

    Task<IReadOnlyList<Subscription>> GetByUserIdAsync(
        Guid userId);

    Task<Subscription> CreateAsync(
        CreateSubscriptionRequest request);
}