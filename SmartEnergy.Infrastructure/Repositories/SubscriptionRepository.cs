using Microsoft.EntityFrameworkCore;
using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly SmartEnergyDbContext _context;

    public SubscriptionRepository(SmartEnergyDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription?> GetByIdAsync(Guid id)
    {
        return await _context.Subscriptions
            .Include(x => x.User)
            .Include(x => x.Address)
            .Include(x => x.Meter)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Subscription?> GetBySubscriptionNumberAsync(
        string subscriptionNumber)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(
                x => x.SubscriptionNumber == subscriptionNumber);
    }

    public async Task<IReadOnlyList<Subscription>> GetAllAsync()
    {
        return await _context.Subscriptions
            .Include(x => x.User)
            .Include(x => x.Address)
            .Include(x => x.Meter)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Subscription>> GetByUserIdAsync(
        Guid userId)
    {
        return await _context.Subscriptions
            .Include(x => x.Address)
            .Include(x => x.Meter)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
    }

    public void Update(Subscription subscription)
    {
        _context.Subscriptions.Update(subscription);
    }

    public void Remove(Subscription subscription)
    {
        _context.Subscriptions.Remove(subscription);
    }
}