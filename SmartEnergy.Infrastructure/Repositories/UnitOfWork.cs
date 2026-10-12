using SmartEnergy.Application.Abstractions.Repositories;

namespace SmartEnergy.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SmartEnergyDbContext _context;

    public UnitOfWork(SmartEnergyDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}