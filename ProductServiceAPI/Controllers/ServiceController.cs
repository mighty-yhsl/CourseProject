using CourseProject.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
using ProductServiceAPI;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly string _connectionString;
    private readonly ServiceStatusCache _statusCache;
    private readonly IServiceRepository _serviceRepository;

    public ServiceController(IConfiguration configuration, ServiceStatusCache statusCache, IServiceRepository serviceRepository)
    {
        _connectionString = configuration.GetConnectionString("ServiceRegisterConnection");
        _statusCache = statusCache;
        _serviceRepository = serviceRepository;
    }


    [HttpGet("GetAllServices")]
    public async Task<IActionResult> GetAllServices()  // Сделали метод асинхронным
    {
        var services = new List<ServiceWithStatus>();

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT s.ServiceId, s.ServiceName, s.Description, s.CreatedDate, s.UpdatedDate, 
                              si.ServiceStatusId, ss.ServiceStatusName
                              FROM Service s
                              LEFT JOIN ServiceInstance si ON s.ServiceId = si.ServiceId
                              LEFT JOIN ServiceStatus ss ON si.ServiceStatusId = ss.ServiceStatusId";
                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();  // Используем асинхронное открытие соединения

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())  // Асинхронный вызов ExecuteReader
                {
                    while (await reader.ReadAsync())  // Асинхронное чтение данных
                    {
                        var service = new ServiceWithStatus
                        {
                            ServiceId = (int)reader["ServiceId"],
                            ServiceName = reader["ServiceName"].ToString(),
                            Description = reader["Description"].ToString(),
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            UpdatedDate = (DateTime)reader["UpdatedDate"],
                            ServiceStatusId = (int)reader["ServiceStatusId"],
                            ServiceStatusName = reader["ServiceStatusName"].ToString()
                        };

                        bool? cachedStatus = _statusCache.GetServiceStatus(service.ServiceId);
                        if (cachedStatus.HasValue && cachedStatus.Value != (service.ServiceStatusName == "Active"))
                        {
                            bool newStatus = cachedStatus.Value;
                            await UpdateServiceStatusInDb(service.ServiceId, newStatus);  // Асинхронный вызов для обновления статуса
                        }

                        services.Add(service);
                    }
                }
            }

            return Ok(services);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private async Task UpdateServiceStatusInDb(int serviceId, bool isRunning)
    {
        int statusId = isRunning ? 1 : 2; // 1 - Active, 2 - Inactive

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


[HttpPost("CreateService")]
    public IActionResult CreateService(string serviceName, string description, string address)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return BadRequest("Service name is required.");
        }

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string queryService = "INSERT INTO Service (ServiceName, Description) " +
                                      "VALUES (@ServiceName, @Description); " +
                                      "SELECT SCOPE_IDENTITY();";

                SqlCommand cmdService = new SqlCommand(queryService, conn);
                cmdService.Parameters.AddWithValue("@ServiceName", serviceName);
                cmdService.Parameters.AddWithValue("@Description", description);

                int serviceId = Convert.ToInt32(cmdService.ExecuteScalar());

                string queryServiceInstance = "INSERT INTO ServiceInstance (ServiceId, ServiceStatusId, Address) " +
                                              "VALUES (@ServiceId, 1, @Address)";

                SqlCommand cmdServiceInstance = new SqlCommand(queryServiceInstance, conn);
                cmdServiceInstance.Parameters.AddWithValue("@ServiceId", serviceId);
                cmdServiceInstance.Parameters.AddWithValue("@Address", address);

                cmdServiceInstance.ExecuteNonQuery();
            }

            return Ok("Service created successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("UpdateService")]
    public IActionResult UpdateService(int? serviceId, string serviceName, string description, string address, bool status)
    {
        if (string.IsNullOrWhiteSpace(serviceName) && serviceId == null)
        {
            return BadRequest("Either serviceId or serviceName is required.");
        }

        int statusId = status ? 1 : 2;  // если status = true, то статус будет Active (1), если false - Inactive (2)

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Если передан serviceId, то обновляем сервис по ID
                string query = string.Empty;
                if (serviceId.HasValue)
                {
                    query = @"UPDATE Service 
                         SET ServiceName = @ServiceName, Description = @Description, 
                             UpdatedDate = GETDATE() 
                         WHERE ServiceId = @ServiceId";
                }
                else
                {
                    query = @"UPDATE Service 
                         SET ServiceName = @ServiceName, Description = @Description, 
                             UpdatedDate = GETDATE() 
                         WHERE ServiceName = @ServiceName";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ServiceName", serviceName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@ServiceId", serviceId.HasValue ? serviceId.Value : (object)DBNull.Value);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound("Service not found.");
                }

                string queryInstance = string.Empty;
                if (serviceId.HasValue)
                {
                    queryInstance = @"UPDATE ServiceInstance
                                 SET Address = @Address, 
                                     ServiceStatusId = @Status, 
                                     UpdatedDate = GETDATE() 
                                 WHERE ServiceId = @ServiceId";
                }
                else
                {
                    queryInstance = @"UPDATE ServiceInstance
                                 SET Address = @Address, 
                                     ServiceStatusId = @Status, 
                                     UpdatedDate = GETDATE() 
                                 WHERE ServiceId = (SELECT ServiceId FROM Service WHERE ServiceName = @ServiceName)";
                }

                SqlCommand cmdInstance = new SqlCommand(queryInstance, conn);
                cmdInstance.Parameters.AddWithValue("@Address", address);
                cmdInstance.Parameters.AddWithValue("@Status", statusId);
                cmdInstance.Parameters.AddWithValue("@ServiceName", serviceName);
                cmdInstance.Parameters.AddWithValue("@ServiceId", serviceId.HasValue ? serviceId.Value : (object)DBNull.Value);

                cmdInstance.ExecuteNonQuery();
            }

            return Ok("Service updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}




