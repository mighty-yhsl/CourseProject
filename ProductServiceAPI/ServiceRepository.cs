using CourseProject.DAL.Models;
using Microsoft.Data.SqlClient;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllServicesAsync();
    Task UpdateServiceStatusAsync(int serviceId, bool isRunning);
}

public class ServiceRepository : IServiceRepository
{
    private readonly string _connectionString;

    public ServiceRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ServiceRegisterConnection");
    }

    public async Task<IEnumerable<Service>> GetAllServicesAsync()
    {
        var services = new List<Service>();

        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = @"
            SELECT s.ServiceId, s.ServiceName, s.Description, si.Address, ss.ServiceStatusName
            FROM Service s
            INNER JOIN ServiceInstance si ON s.ServiceId = si.ServiceId
            INNER JOIN ServiceStatus ss ON si.ServiceStatusId = ss.ServiceStatusId";

            using (var cmd = new SqlCommand(query, conn))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var service = new Service
                        {
                            ServiceId = reader.GetInt32(0),
                            ServiceName = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Address = reader.GetString(3),
                            ServiceStatusName = reader.GetString(4) // Статус из таблицы ServiceStatus
                        };
                        services.Add(service);
                    }
                }
            }
        }

        return services;
    }

    public async Task UpdateServiceStatusAsync(int serviceId, bool isRunning)
    {
        int statusId = isRunning ? 1 : 2; 

        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = @"
            UPDATE ServiceInstance 
            SET ServiceStatusId = @StatusId 
            WHERE ServiceId = @ServiceId";

            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StatusId", statusId);
                cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}