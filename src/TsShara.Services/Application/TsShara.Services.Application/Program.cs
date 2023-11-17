using Codescovery.Library.DependencyInjection.Extensions;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAppSettings<AppSettings>(builder.Configuration);

builder.Services.AddDefaultAvailableSerialUsbPorts();
builder.Services.AddDefaultTsSharaStatusFromUsb();
builder.Services.AddDefaultTsSharaInformationDataService();
builder.Services.AddTimeSpanServices(ServiceLifetime.Singleton);
builder.Services.AddFeatures();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
