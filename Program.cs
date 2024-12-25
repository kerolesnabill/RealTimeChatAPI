using RealTimeChatAPI.Data.Seeders;
using RealTimeChatAPI.Extensions;
using RealTimeChatAPI.Hubs;
using RealTimeChatAPI.Middlewares;
using Scalar.AspNetCore;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddServices(builder.Configuration);

    var app = builder.Build();

    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
    await seeder.Seed();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseCors("AllowOrigins");

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<ChatHub>("ChatHub");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}