using Microsoft.EntityFrameworkCore;
using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence.Repositories;

public class BillRepository : IBillRepository
{
    private readonly SmartEnergyDbContext _context;

    public BillRepository(SmartEnergyDbContext context)
    {
        _context = context;
    }

    public async Task<Bill?> GetByIdAsync(Guid id)
    {
        return await _context.Bills
            .Include(x => x.Subscription)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Bill?> GetByBillNumberAsync(
        string billNumber)
    {
        return await _context.Bills
            .FirstOrDefaultAsync(
                x => x.BillNumber == billNumber);
    }

    public async Task<IReadOnlyList<Bill>> GetAllAsync()
    {
        return await _context.Bills
            .Include(x => x.Subscription)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Bill>>
        GetBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await _context.Bills
            .AsNoTracking()
            .Where(x => x.SubscriptionId == subscriptionId)
            .ToListAsync();
    }

    public async Task AddAsync(Bill bill)
    {
        await _context.Bills.AddAsync(bill);
    }

    public void Update(Bill bill)
    {
        _context.Bills.Update(bill);
    }

    public void Remove(Bill bill)
    {
        _context.Bills.Remove(bill);
    }
}