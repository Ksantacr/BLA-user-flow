using BLA.UserFlow.Application.DTO;

namespace BLA.UserFlow.Application.Services.TokenHandler;

public interface ITokenHandler
{
    string CreateJWTToken(UserByEmailDto user);
}