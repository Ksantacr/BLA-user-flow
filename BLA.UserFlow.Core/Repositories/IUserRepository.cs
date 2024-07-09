using BLA.UserFlow.Core.Entities;

namespace BLA.UserFlow.Core.Repositories;

public interface IUserRepository
{
    Task<User?> RegisterUserAsync(User model);
    Task<User?> GetUserByEmailAsync(string email);
}