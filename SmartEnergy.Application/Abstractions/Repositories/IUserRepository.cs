using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);

    Task<IReadOnlyList<User>> GetAllAsync();

    Task AddAsync(User user);

    void Update(User user);

    void Remove(User user);
}