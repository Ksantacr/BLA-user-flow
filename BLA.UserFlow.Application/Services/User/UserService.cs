using BLA.UserFlow.Application.DTO;
using BLA.UserFlow.Application.OperationResult;
using BLA.UserFlow.Core.Repositories;

namespace BLA.UserFlow.Application.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<RegisterUserResponseDto>> RegisterUserAsync(RegisterUserRequestDto model)
    {
        var user = await _userRepository.RegisterUserAsync(
            model.DtoToUser(BCrypt.Net.BCrypt.HashPassword(model.Password)));
        if (user is null)
        {
            return Result<RegisterUserResponseDto>.Failure();
        }

        return Result<RegisterUserResponseDto>.Success(RegisterUserResponseDto.UserToDto(user));
    }

    public async Task<UserByEmailDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user is null) return null;
        return UserByEmailDto.UserToDto(user);
    }

    public async Task<bool> IsValidPassword(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user is not null)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        return false;
    }
}