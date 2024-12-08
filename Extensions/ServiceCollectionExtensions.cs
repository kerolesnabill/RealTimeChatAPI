using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealTimeChatAPI.Data;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Helpers;
using RealTimeChatAPI.Middlewares;
using System.Text;

namespace RealTimeChatAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddControllers();
        services.AddOpenApi();

        services.AddCors(options =>
        {
            var origins = configuration.GetSection("Origins").Get<string[]>();
            options.AddPolicy("AllowOrigins", policyBuilder =>
                policyBuilder.WithOrigins(origins!)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
        });

        // Application services
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly).AddFluentValidationAutoValidation();
        services.AddAutoMapper(assembly);

        services.AddScoped<JwtHelper>();

        // Data(Infrastructure) services
        services.AddDbContext<RealTimeChatDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("RealTimeChatDb"))
                .EnableSensitiveDataLogging();
        });

        services.AddScoped<IUsersRepository, UsersRepository>();
    }
}
