using Microsoft.OpenApi.Models;
using ProductServiceAPI;

var builder = WebApplication.CreateBuilder(args);

// ���������� ��������
builder.Services.AddControllers();

// ���������� �����������
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// ���������� ���� ������ �������� �������
builder.Services.AddHostedService<ServiceHealthCheckBackgroundService>();
builder.Services.AddSingleton<ServiceStatusCache>();

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

// ���������� HttpClient
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