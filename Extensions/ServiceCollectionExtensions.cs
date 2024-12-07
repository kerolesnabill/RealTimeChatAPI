using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.Data;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Middlewares;

namespace RealTimeChatAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddControllers();
        services.AddOpenApi();

        // Application services
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly).AddFluentValidationAutoValidation();
        services.AddAutoMapper(assembly);

        // Data(Infrastructure) services
        services.AddDbContext<RealTimeChatDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("RealTimeChatDb"))
                .EnableSensitiveDataLogging();
        });

        services.AddScoped<IUsersRepository, UsersRepository>();
    }
}
