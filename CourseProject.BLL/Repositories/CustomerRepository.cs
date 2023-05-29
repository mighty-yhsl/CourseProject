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

namespace CourseProject.BLL.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        private const string CREATE_QUERY = "INSERT INTO Customer(CustomerName, CustomerSurname, Phone, Email, Addres) Values(@CustomerName, @CustomerSurname, @Phone, @Email, @Addres)";
        private const string DELETE_QUERY = "DELETE FROM Customer WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE Customer SET CustomerName = @CustomerName, CustomerSurname = @CustomerSurname, Phone = @Phone, Email = @Email, Addres = @Addres WHERE Id = @Id";
        private const string GET_BY_ID_QUERY = "SELECT c.Id, c.CustomerName, c.CustomerSurname, c.Phone, c.Email, c.Addres FROM Customer c WHERE c.Id = @Id";
        private const string GET_QUERY = "SELECT c.Id, c.CustomerName, c.CustomerSurname, c.Phone, c.Email, c.Addres FROM Customer c";


        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public CustomerRepository() { }

        public override void Create(Customer entity)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerName", entity.CustomerName),
                new SqlParameter("@CustomerSurname", entity.CustomerSurname),
                new SqlParameter("@Phone", entity.Phone),
                new SqlParameter("@Email", entity.Email),
                new SqlParameter("@Addres", entity.Addres)
            };
            ExecuteScalarCommand(CREATE_QUERY, parameters);
        }

        public override void Delete(Customer entity)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@Id", entity.Id) };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        public override Customer Get(int id)
        {
            Customer customer = new Customer();
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
                            customer.Id = Convert.ToInt32(reader["Id"]);
                            customer.CustomerName = Convert.ToString(reader["CustomerName"]);
                            customer.CustomerSurname = Convert.ToString(reader["CustomerSurname"]);
                            customer.Phone = Convert.ToString(reader["Phone"]);
                            customer.Email = Convert.ToString(reader["Email"]);
                            customer.Addres = Convert.ToString(reader["Addres"]);
                        }
                    }
                }
                connection.Close();
            }
            return customer;
        }

        public override IEnumerable<Customer> Get()
        {
            List<Customer> customers = new List<Customer>();

            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand(GET_QUERY, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer();
                            customer.Id = Convert.ToInt32(reader["Id"]);
                            customer.CustomerName = Convert.ToString(reader["CustomerName"]);
                            customer.CustomerSurname = Convert.ToString(reader["CustomerSurname"]);
                            customer.Phone = Convert.ToString(reader["Phone"]);
                            customer.Email = Convert.ToString(reader["Email"]);
                            customer.Addres = Convert.ToString(reader["Addres"]);

                            customers.Add(customer);
                        }
                    }
                }
            }
            return customers;
        }

        public override void Update(Customer entity)
        {
            var parameters = new SqlParameter[] {
            new SqlParameter("@CustomerName", entity.CustomerName),
            new SqlParameter("@CustomerSurname", entity.CustomerSurname),
            new SqlParameter("@Phone", entity.Phone),
            new SqlParameter("@Email", entity.Email),
            new SqlParameter("@Addres", entity.Addres),
            };
            ExecuteCommand(UPDATE_QUERY, parameters);

        }
    }
}
