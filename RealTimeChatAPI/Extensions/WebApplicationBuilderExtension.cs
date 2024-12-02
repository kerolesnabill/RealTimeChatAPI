﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealTimeChatAPI.Middlewares;
using System.Text;

namespace RealTimeChatAPI.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                x.RequireHttpsMetadata = false;

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        if(!string.IsNullOrEmpty(token))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddCors(options =>
        {
            var origins = builder.Configuration.GetSection("Origins").Get<string[]>();
            options.AddPolicy("AllowOrigins", policyBuilder =>
                policyBuilder.WithOrigins(origins!)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
        });

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme 
                        {
                            Reference = new OpenApiReference
                                {Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
                        },
                        []
                    }
                });
        });

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
    }
}
