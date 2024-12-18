using Microsoft.OpenApi.Models;
using ProductServiceAPI;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов
builder.Services.AddControllers();

// Добавление репозитория
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// Добавление фона службы проверки статуса
builder.Services.AddHostedService<ServiceHealthCheckBackgroundService>();
builder.Services.AddSingleton<ServiceStatusCache>();

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

// Добавление HttpClient
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