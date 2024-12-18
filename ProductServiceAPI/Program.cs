using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using ProductServiceAPI;

var builder = WebApplication.CreateBuilder(args);

// ��������� ����������� � �������������� NLog
builder.Logging.ClearProviders();  // ������� ����������� ���������� �����������
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);  // ��������� ����������� ������� �����������
builder.Host.UseNLog();  // ���������, ��� ����� ������������ NLog

// ���������� ��������
builder.Services.AddControllers();

// ���������� �����������
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// ���������� ���� ������ �������� �������
builder.Services.AddHostedService<ServiceHealthCheckBackgroundService>();
builder.Services.AddSingleton<ServiceStatusCache>();

// ������������ PingBackgroundService ��� ������� ������
builder.Services.AddHostedService<PingBackgroundService>(sp =>
    new PingBackgroundService("ServiceRegisterConnection"));

// ���������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProductService API",
        Version = "v1"
    });
});
builder.Services.AddHttpClient();
var app = builder.Build();

// ������������ HTTP �������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// ������ ����������
app.Run();