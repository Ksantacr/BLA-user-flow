using BLA.UserFlow.Application.DTO;
using BLA.UserFlow.Application.OperationResult;

namespace BLA.UserFlow.Application.Services.User;

public interface IUserService
{
    Task<Result<RegisterUserResponseDto>> RegisterUserAsync(RegisterUserRequestDto model);
    Task<UserByEmailDto?> GetUserByEmailAsync(string email);
    Task<bool> IsValidPassword(string email, string password);
}