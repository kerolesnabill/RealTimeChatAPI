using RealTimeChatAPI.Extensions;
using RealTimeChatAPI.Hubs;
using RealTimeChatAPI.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowOrigins");

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("ChatHub");

app.Run();
