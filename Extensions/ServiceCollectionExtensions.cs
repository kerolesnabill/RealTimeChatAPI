using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.Data;

namespace RealTimeChatAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        services.AddDbContext<RealTimeChatDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("RealTimeChatDb"))
                .EnableSensitiveDataLogging();
        });
    }
}
