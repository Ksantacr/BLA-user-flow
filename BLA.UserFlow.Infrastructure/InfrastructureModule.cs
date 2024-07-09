using System.Text;
using BLA.UserFlow.Core.Repositories;
using BLA.UserFlow.Infrastructure.DatabaseConnection;
using BLA.UserFlow.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BLA.UserFlow.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddDatabaseDependency()
            .AddAuthDependencies()
            .AddServiceDependencies();
    }

    public static IServiceCollection AddDatabaseDependency(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<DbConnectionProvider>(sp =>
            {
                var connectionString = Environment.GetEnvironmentVariable("BLACONNECTIONSTRING");
                return new DbConnectionProvider(connectionString);
            }
        ); 
    }

    public static IServiceCollection AddAuthDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWTISSUER"),
                    ValidAudience = Environment.GetEnvironmentVariable("JWTAUDIENCE"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            Environment.GetEnvironmentVariable("JWTKEY")))
                });

        return serviceCollection;
    }

    public static IServiceCollection AddServiceDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IPostRepository, PostRepository>();

        return serviceCollection;
    }
}