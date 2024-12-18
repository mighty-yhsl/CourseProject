using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using ProductServiceAPI;

var builder = WebApplication.CreateBuilder(args);

// Настройка логирования с использованием NLog
builder.Logging.ClearProviders();  // Убираем стандартные провайдеры логирования
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);  // Установим минимальный уровень логирования
builder.Host.UseNLog();  // Указываем, что будем использовать NLog

// Добавление сервисов
builder.Services.AddControllers();

// Добавление репозитория
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// Добавление фона службы проверки статуса
builder.Services.AddHostedService<ServiceHealthCheckBackgroundService>();
builder.Services.AddSingleton<ServiceStatusCache>();

// Регистрируем PingBackgroundService как фоновый сервис
builder.Services.AddHostedService<PingBackgroundService>(sp =>
    new PingBackgroundService("ServiceRegisterConnection"));

// Добавление Swagger
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

// Конфигурация HTTP запроса
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Запуск приложения
app.Run();