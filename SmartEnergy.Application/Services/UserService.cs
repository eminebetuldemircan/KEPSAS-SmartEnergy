using SmartEnergy.Application.Abstractions.Repositories;
using SmartEnergy.Application.DTOs.Users;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User> CreateAsync(CreateUserRequest request)
    {
        var existingUser = await _userRepository
            .GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                "Bu e-posta adresi zaten kayıtlı.");
        }

        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        await _userRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        return user;
    }
}