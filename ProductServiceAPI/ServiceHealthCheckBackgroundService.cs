using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductServiceAPI;

public class ServiceHealthCheckBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;
    private readonly ServiceStatusCache _statusCache;

    public ServiceHealthCheckBackgroundService(
        IServiceProvider serviceProvider,
        ServiceStatusCache statusCache,
        IHttpClientFactory httpClientFactory)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _statusCache = statusCache ?? throw new ArgumentNullException(nameof(statusCache));

        _httpClient = httpClientFactory.CreateClient("ServiceHealthClient");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAllServices(stoppingToken);
            }
            catch
            {
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
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

            _statusCache.UpdateServiceStatus(service.ServiceId, status);
        }
    }

    private async Task<bool> CheckServiceStatusAsync(string address)
    {
        try
        {
            var response = await _httpClient.GetAsync(address);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
