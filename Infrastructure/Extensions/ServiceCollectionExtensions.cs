using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ChatDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ChatDb"));
        });

        services.AddScoped<IUsersRepository, UsersRepository>();
    }
}
