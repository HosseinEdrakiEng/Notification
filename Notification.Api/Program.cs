using Infrastructure;
using Notification.Api;
using Notification.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddConfigOption(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddWorker();
builder.Services.AddProviderHttpClient(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddMiddleware();
builder.Services.AddApiVersion();
builder.Services.AddSerilogger(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
