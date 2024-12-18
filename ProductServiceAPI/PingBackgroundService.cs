using Microsoft.Extensions.Hosting;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.DAL.Models.EF;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

public class PingBackgroundService : BackgroundService
{
    private readonly string _connectionString;

    public PingBackgroundService(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckServicesAvailabilityAsync();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Повтор каждые 1 минуту
        }
    }

    private async Task CheckServicesAvailabilityAsync()
    {
        try
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = @"SELECT ServiceId, Address FROM ServiceInstance";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int serviceId = reader.GetInt32(0);
                        string address = reader.GetString(1);

                        bool isReachable = await PingAddressAsync(address);
                        await UpdateServiceStatusInDb(serviceId, isReachable, conn);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in PingBackgroundService: {ex.Message}");
        }
    }

    private async Task<bool> PingAddressAsync(string address)
    {
        using (var ping = new Ping())
        {
            try
            {
                var reply = await ping.SendPingAsync(address, 1000); // Таймаут 1 секунда
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }

    private async Task UpdateServiceStatusInDb(int serviceId, bool isReachable, SqlConnection conn)
    {
        string updateQuery = @"UPDATE ServiceInstance 
                               SET ServiceStatusId = @Status 
                               WHERE ServiceId = @ServiceId";

        using (var updateCmd = new SqlCommand(updateQuery, conn))
        {
            updateCmd.Parameters.AddWithValue("@ServiceId", serviceId);
            updateCmd.Parameters.AddWithValue("@Status", isReachable ? 1 : 2);

            await updateCmd.ExecuteNonQueryAsync();
        }
    }
}