using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.DTOs.Subscriptions;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository,
        IUnitOfWork unitOfWork)
    {
        _subscriptionRepository = subscriptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Subscription?> GetByIdAsync(Guid id)
    {
        return await _subscriptionRepository.GetByIdAsync(id);
    }

    public async Task<Subscription?> GetBySubscriptionNumberAsync(
        string subscriptionNumber)
    {
        return await _subscriptionRepository
            .GetBySubscriptionNumberAsync(subscriptionNumber);
    }

    public async Task<IReadOnlyList<Subscription>> GetAllAsync()
    {
        return await _subscriptionRepository.GetAllAsync();
    }

    public async Task<IReadOnlyList<Subscription>> GetByUserIdAsync(
        Guid userId)
    {
        return await _subscriptionRepository
            .GetByUserIdAsync(userId);
    }

    public async Task<Subscription> CreateAsync(
        CreateSubscriptionRequest request)
    {
        var existingSubscription =
            await _subscriptionRepository
                .GetBySubscriptionNumberAsync(
                    request.SubscriptionNumber);

        if (existingSubscription is not null)
        {
            throw new InvalidOperationException(
                "Bu abonelik numarası zaten kayıtlı.");
        }

        var subscription = new Subscription(
            request.UserId,
            request.AddressId,
            request.MeterId,
            request.SubscriptionNumber,
            request.ContractNumber,
            request.StartDate);

        await _subscriptionRepository.AddAsync(subscription);

        await _unitOfWork.SaveChangesAsync();

        return subscription;
    }
}