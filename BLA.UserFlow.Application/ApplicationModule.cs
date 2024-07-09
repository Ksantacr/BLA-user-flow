using BLA.UserFlow.Application.Services.Posts;
using BLA.UserFlow.Application.Services.TokenHandler;
using BLA.UserFlow.Application.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace BLA.UserFlow.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddAplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        serviceCollection.AddScoped<IPostService, PostService>();

        return serviceCollection;
    }
}