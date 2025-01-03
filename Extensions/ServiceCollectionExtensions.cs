﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealTimeChatAPI.Data;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Data.Seeders;
using RealTimeChatAPI.Helpers;
using RealTimeChatAPI.Middlewares;
using RealTimeChatAPI.Services.Users;
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

                x.RequireHttpsMetadata = false;

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
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

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();

        services.AddSignalR();

        // Data(Infrastructure) services
        services.AddDbContext<RealTimeChatDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("RealTimeChatDb"))
                .EnableSensitiveDataLogging();
        });

        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IMessagesRepository, MessagesRepository>();

        services.AddScoped<Seeder>();
    }
}
