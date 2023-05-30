using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.DAL.Models.EF;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection.Metadata;
using System.Diagnostics;

namespace CourseProject.BLL.Repositories
{
    public sealed class TransportRepository : Repository<Transport>
    {
        private const string CREATE_QUERY = "INSERT INTO Transport([Name], Speed, Weightt, EnginePower, Amount, Price, CategoryId, ManufacturerId) VALUES(@Name, @Speed, @Weightt, @EnginePower, @Amount, @Price, @CategoryId, @ManufacturerId)";
        private const string DELETE_QUERY = "DELETE FROM Transport WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE Transport SET [Name] = @Name, Speed = @Speed, Weightt = @Weightt, EnginePower = @EnginePower,Amount = @Amount, Price = @Price, CategoryId = @CategoryId, ManufacturerId = @ManufacturerId WHERE Id = @Id";
        private const string GET_BY_ID_QUERY = "SELECT t.Id, t.Name, t.Speed, t.Weightt, t.EnginePower, t.Amount, t.Price, c.CategoryName AS Category_Name , m.ManufacturerName AS Manufacturer_Name FROM Transport t INNER JOIN Category c ON t.CategoryId = c.Id INNER JOIN Manufacturer m ON t.ManufacturerId = m.Id WHERE t.Id = @Id";
        private const string GET_QUERY = "SELECT t.Name, t.Speed, t.Weightt, t.EnginePower, t.Amount, t.Price,t.CategoryId, t.ManufacturerId, c.CategoryName AS Category_Name, m.ManufacturerName AS Manufacturer_Name FROM Transport t INNER JOIN Category c ON t.CategoryId = c.Id INNER JOIN Manufacturer m ON t.ManufacturerId = m.Id";
        
        public TransportRepository(IConfiguration configuration) : base(configuration)
        {

        }
       
        public TransportRepository() { }

        public override void Create(Transport entity)
        {
            if (entity == null || entity.Category == null || entity.Manufacturer == null)
            {
                throw new ArgumentException("Invalid transport entity.");
            }
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", entity.Name),
                new SqlParameter("@Speed", entity.Speed),
                new SqlParameter("@Weightt", entity.Weightt),
                new SqlParameter("@EnginePower", entity.EnginePower),
                new SqlParameter("@Amount", entity.Amount),
                new SqlParameter("@Price", entity.Price),
                new SqlParameter("@CategoryId", entity.Category.Id),
                new SqlParameter("@ManufacturerId", entity.Manufacturer.Id)
            };
            ExecuteCommand(CREATE_QUERY, parameters);
        }
        
  

        public override void Delete(Transport entity)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@Id", entity.Id) };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        public override Transport Get(int id)
        {
            Transport transport = new Transport();
            using (var connection = new SqlConnection(con))
            {
                SqlParameter parameter = new SqlParameter("@Id", id);
                connection.Open();
                using (var command = new SqlCommand(GET_BY_ID_QUERY, connection))
                {
                    command.Parameters.Add(parameter);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            transport.Id = Convert.ToInt32(reader["Id"]);
                            transport.Name = Convert.ToString(reader["Name"]);
                            transport.Speed = Convert.ToInt32(reader["Speed"]);
                            transport.Weightt = Convert.ToInt32(reader["Weightt"]);
                            transport.EnginePower = Convert.ToInt32(reader["EnginePower"]);
                            transport.Amount = Convert.ToInt32(reader["Amount"]);
                            transport.Price = Convert.ToDecimal(reader["Price"]);
                            transport.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                            transport.Category = new Category { Id = (int)reader["CategoryId"], CategoryName = (string)reader["Category_Name"] };
                            transport.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                            transport.Manufacturer = new Manufacturer { Id = (int)reader["ManufacturerId"], ManufacturerName = (string)reader["Manufacturer_Name"] };
                        }
                    }
                }
                connection.Close();
            }
            return transport;
        }

        public override IEnumerable<Transport> Get()
        {
            List<Transport> transports = new List<Transport>();
       
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand(GET_QUERY, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transport transport = new Transport();
                            transport.Name = Convert.ToString(reader["Name"]);
                            transport.Speed = Convert.ToInt32(reader["Speed"]);
                            transport.Weightt = Convert.ToInt32(reader["Weightt"]);
                            transport.EnginePower = Convert.ToInt32(reader["EnginePower"]);
                            transport.Amount = Convert.ToInt32(reader["Amount"]);
                            transport.Price = Convert.ToDecimal(reader["Price"]);
                            transport.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                            transport.Category = new Category { Id = (int)reader["CategoryId"], CategoryName = (string)reader["Category_Name"] };
                            transport.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                            transport.Manufacturer = new Manufacturer { Id = (int)reader["ManufacturerId"], ManufacturerName = (string)reader["Manufacturer_Name"] };

                            transports.Add(transport);
                        }
                    }
                }
            }
            return transports;
        }

        public override void Update(Transport entity)
        {
            var parameters = new SqlParameter[] {
            new SqlParameter("@Name", entity.Name),
            new SqlParameter("@Speed", entity.Speed),
            new SqlParameter("@Weightt", entity.Weightt),
            new SqlParameter("@EnginePower", entity.EnginePower),
            new SqlParameter("@Amount", entity.Amount),
            new SqlParameter("@Price", entity.Price),
            new SqlParameter("@CategoryId", entity.CategoryId),
            new SqlParameter("@ManufacturerId", entity.ManufacturerId)
            };
            ExecuteCommand(UPDATE_QUERY, parameters);

        }
    }
}