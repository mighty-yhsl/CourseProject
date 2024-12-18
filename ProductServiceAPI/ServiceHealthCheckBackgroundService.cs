using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductServiceAPI;

public class ServiceHealthCheckBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;
    private readonly ServiceStatusCache _statusCache;
    private readonly ILogger<ServiceHealthCheckBackgroundService> _logger;

    public ServiceHealthCheckBackgroundService(
        IServiceProvider serviceProvider,
        ServiceStatusCache statusCache,
        ILogger<ServiceHealthCheckBackgroundService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _statusCache = statusCache ?? throw new ArgumentNullException(nameof(statusCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _httpClient = httpClientFactory.CreateClient("ServiceHealthClient");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Service Health Check Background Service запущен...");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Начата фоновая проверка в {Time}", DateTimeOffset.Now);

            try
            {
                await CheckAllServices(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка во время выполнения фоновой проверки.");
            }

            _logger.LogInformation("Фоновая проверка завершена в {Time}", DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        _logger.LogInformation("Service Health Check Background Service остановлен.");
    }

    private async Task CheckAllServices(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var serviceRepository = scope.ServiceProvider.GetRequiredService<IServiceRepository>();
        var services = await serviceRepository.GetAllServicesAsync();

        foreach (var service in services)
        {
            if (stoppingToken.IsCancellationRequested) break;

            var status = await CheckServiceStatusAsync(service.Address);
            service.IsServiceRunning = status;

            // Логируем статус сервиса
            _logger.LogInformation("Service ID {ServiceId}, Address {Address}, Status: {Status}",
                service.ServiceId, service.Address, status);

            // Обновляем статус в кэше
            _statusCache.UpdateServiceStatus(service.ServiceId, status);
        }
    }

    private async Task<bool> CheckServiceStatusAsync(string address)
    {
        try
        {
            // Делаем запрос на доступность сервиса
            var response = await _httpClient.GetAsync(address);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка проверки доступности сервиса: {Address}", address);
            return false; // Возвращаем false при любой ошибке
        }
    }
}