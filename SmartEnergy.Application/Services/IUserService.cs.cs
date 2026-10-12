using SmartEnergy.Application.DTOs.Users;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);

    Task<IReadOnlyList<User>> GetAllAsync();

    Task<User> CreateAsync(CreateUserRequest request);
}