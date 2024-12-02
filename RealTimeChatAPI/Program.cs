using Infrastructure.Extensions;
using Application.Extensions;
using RealTimeChatAPI.Middlewares;
using RealTimeChatAPI.Extensions;
using Application.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigins");

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("ChatHub");

app.Run();
