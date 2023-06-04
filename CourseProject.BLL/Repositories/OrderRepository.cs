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
using CourseProject.BLL.Services;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace CourseProject.BLL.Repositories
{
    public class OrderRepository : Repository<CustomerOrder>
    {
        private const string CREATE_QUERY = "INSERT INTO CustomerOrder (Description, CreateDate, UpdateDate, SellerId, StatusId, CustomerId) OUTPUT INSERTED.ID, SCOPE_IDENTITY() VALUES (@Description, @CreateDate, @UpdateDate, @SellerId, @StatusId, @CustomerId)";
        private const string DELETE_QUERY = "DELETE FROM CustomerOrder WHERE Id = @Id";
        private const string DELETE_QUERY_DETAILS = "DELETE FROM OrderDetails WHERE TransportId = @TransportId AND CustomerOrderId = @CustomerOrderId";
        private const string UPDATE_QUERY = "UPDATE CustomerOrder SET Description = @Description, CreateDate = @CreateDate, UpdateDate = @UpdateDate, SellerId = @SellerId, CustomerId = @CustomerId, StatusId = @StatusId WHERE Id = @Id";
        private const string UPDATE_QUERY_DETAILS = "UPDATE OrderDetails SET TotalAmount = @TotalAmount, TotalPrice = @TotalPrice WHERE CustomerOrderId = @CustomerOrderId, TransportId = @TransportId";
        private const string GET_BY_ID_QUERY = "SELECT co.Id, co.Description, co.CreateDate, co.UpdateDate, co.SellerId, co.CustomerId, co.StatusId, od.Id AS OrderDetail_Id, od.TotalAmount, od.TotalPrice, od.CustomerOrderId, od.TransportId, so.Id AS Status_Id, so.StatusOrderName, t.Name AS Transport_Name FROM CustomerOrder co LEFT JOIN OrderDetails od ON co.Id = od.CustomerOrderId LEFT JOIN Transport t ON od.TransportId = t.Id LEFT JOIN StatusOrder so ON co.StatusId = so.Id WHERE co.Id = @Id";
        private const string GET_QUERY = "SELECT co.Id, c.CustomerName,c.CustomerSurname, c.Addres, c.Email, c.Phone, co.Description, co.CreateDate, co.UpdateDate, so.StatusOrderName,so.StatusOrderName, od.TotalAmount, od.TotalPrice FROM CustomerOrder co JOIN Customer c ON co.CustomerId = c.Id JOIN OrderDetails od ON co.Id = od.CustomerOrderId JOIN StatusOrder so ON co.StatusId = so.Id";
        private const string GET_QUERY_DETAILS = "SELECT TransportId, CustomerOrderId, TotalAmount, OrderDetails.TotalPrice, Transport.Name as 'Title' FROM OrderDetails INNER JOIN Transport ON OrderDetails.TransportId = Transport.Id WHERE CustomerOrderId = @CustomerOrderId AND TransportId = @TransportId";
        private const string CREATE_QUERY_ORDERDETAIL = "INSERT INTO OrderDetails (TotalAmount, TotalPrice, CustomerOrderId, TransportId) VALUES (@TotalAmount, @TotalPrice, @CustomerOrderId, @TransportId)";
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {

        }
       
        public OrderRepository() { }


        public override void Create(CustomerOrder entity)
        {
            var parameters = new List<SqlParameter>()
             {
                 new SqlParameter("@CreateDate", entity.CreateDate),
                 new SqlParameter("@UpdateDate", entity.UpdateDate),
                new SqlParameter("@SellerId", entity.SellerId),
                 new SqlParameter("@StatusId", entity.StatusId),
                 new SqlParameter("@CustomerId", entity.Customer.Id),
                 new SqlParameter("@Description", entity.Description.IsNullOrEmpty() ? DBNull.Value : (object)entity.Description)
            };

            var outputCustomerIdParameter = new SqlParameter("@InsertedCustomerId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            parameters.Add(outputCustomerIdParameter);

            var id = ExecuteScalarCommand(CREATE_QUERY, parameters.ToArray());

            foreach (var item in entity.OrderDetails) 
            {
                item.CustomerOrderId = (int)id;
                CreateDetails(item);
            }

        }

        public void CreateDetails(OrderDetail order)
        {
            var parameters = new SqlParameter[]
            {
        new SqlParameter("@TransportId", order.TransportId),
        new SqlParameter("@CustomerOrderId", order.CustomerOrderId),
        new SqlParameter("@TotalAmount", order.TotalAmount),
        new SqlParameter("@TotalPrice", order.TotalPrice)
            };
            ExecuteScalarCommand(CREATE_QUERY_ORDERDETAIL, parameters);
        }

        public override void Delete(CustomerOrder entity)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@Id", entity.Id) };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        public void DeleteDetails(int TransportId, int CustomerOrderId)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@TransportId", TransportId), new SqlParameter("@CustomerOrderId", CustomerOrderId) };
            ExecuteCommand(DELETE_QUERY_DETAILS, parameters);
        }

        public override CustomerOrder Get(int id)
        {
            CustomerOrder customerOrder = null;

            using (var connection = new SqlConnection(con))
            {
                connection.Open();

                using (var command = new SqlCommand(GET_BY_ID_QUERY, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customerOrder = new CustomerOrder
                            {
                                Id = GetInt32Value(reader, "Id"),
                                Description = GetString(reader, "Description"),
                                CreateDate = GetDateTimeValue(reader, "CreateDate"),
                                UpdateDate = GetDateTimeValue(reader, "UpdateDate"),
                                SellerId = GetInt32Value(reader, "SellerId"),
                                CustomerId = GetInt32Value(reader, "CustomerId"),
                                StatusId = GetInt32Value(reader, "StatusId"),
                                OrderDetails = new List<OrderDetail>
                        {
                            new OrderDetail
                            {
                                TotalAmount = GetInt32Value(reader, "TotalAmount"),
                                TotalPrice = GetDecimalValue(reader, "TotalPrice"),
                              
                            }
                        }
                            };
                        }
                    }
                }
            }

            return customerOrder;
        }

        private int GetInt32Value(SqlDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? 0 : reader.GetInt32(reader.GetOrdinal(columnName));
        }

        private string GetString(SqlDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? string.Empty : reader.GetString(reader.GetOrdinal(columnName));
        }

        private DateTime GetDateTimeValue(SqlDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal(columnName));
        }

        private decimal GetDecimalValue(SqlDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? 0 : reader.GetDecimal(reader.GetOrdinal(columnName));
        }


        public override IEnumerable<CustomerOrder> Get()
        {
            List<CustomerOrder> customerOrders = new List<CustomerOrder>();

            using (var connection = new SqlConnection(con))
            {
                connection.Open();

                using (var command = new SqlCommand(GET_QUERY, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerOrder customerOrder = new CustomerOrder()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                                UpdateDate = reader.GetDateTime(reader.GetOrdinal("UpdateDate")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                Customer = new Customer()
                                {
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    CustomerSurname = reader.GetString(reader.GetOrdinal("CustomerSurname")),
                                    Addres = reader.GetString(reader.GetOrdinal("Addres")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone"))
                                },
                                StatusOrder = new StatusOrder()
                                {
                                    StatusOrderName = reader.GetString(reader.GetOrdinal("StatusOrderName"))
                                }
                            };

                            OrderDetail detail = new OrderDetail()
                            {
                                TotalAmount = reader.GetInt32(reader.GetOrdinal("TotalAmount")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"))
                            };

                            customerOrder.OrderDetails.Add(detail);

                            customerOrders.Add(customerOrder);
                        }
                    }
                }
            }

            return customerOrders;
        }


        public void Update(CustomerOrder entity, Transport transport, Customer customer)
        {
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                       
                        var orderUpdateQuery = "UPDATE CustomerOrder SET Description = @Description, CreateDate = @CreateDate, UpdateDate = @UpdateDate WHERE Id = @OrderId";
                        var orderUpdateCommand = new SqlCommand(orderUpdateQuery, connection, transaction);
                        orderUpdateCommand.Parameters.AddWithValue("@Description", entity.Description);
                        orderUpdateCommand.Parameters.AddWithValue("@CreateDate", entity.CreateDate);
                        orderUpdateCommand.Parameters.AddWithValue("@UpdateDate", entity.UpdateDate);
                        orderUpdateCommand.Parameters.AddWithValue("@OrderId", entity.Id);
                        orderUpdateCommand.ExecuteNonQuery();

                        
                        var transportUpdateQuery = "UPDATE Transport SET Name = @Name, Price = @Price, Amount = @Amount WHERE Id = @TransportId";
                        var transportUpdateCommand = new SqlCommand(transportUpdateQuery, connection, transaction);
                        transportUpdateCommand.Parameters.AddWithValue("@Name", transport.Name);
                        transportUpdateCommand.Parameters.AddWithValue("@Price", transport.Price);
                        transportUpdateCommand.Parameters.AddWithValue("@Amount", transport.Amount);
                        transportUpdateCommand.Parameters.AddWithValue("@TransportId", transport.Id);
                        transportUpdateCommand.ExecuteNonQuery();

                        
                        var customerUpdateQuery = "UPDATE Customer SET CustomerName = @CustomerName, CustomerSurname = @CustomerSurname, Addres = @Addres, Email = @Email, Phone = @Phone WHERE Id = @CustomerId";
                        var customerUpdateCommand = new SqlCommand(customerUpdateQuery, connection, transaction);
                        customerUpdateCommand.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                        customerUpdateCommand.Parameters.AddWithValue("@CustomerSurname", customer.CustomerSurname);
                        customerUpdateCommand.Parameters.AddWithValue("@Addres", customer.Addres);
                        customerUpdateCommand.Parameters.AddWithValue("@Email", customer.Email);
                        customerUpdateCommand.Parameters.AddWithValue("@Phone", customer.Phone);
                        customerUpdateCommand.Parameters.AddWithValue("@CustomerId", customer.Id);
                        customerUpdateCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update entities. Transaction rolled back.", ex);
                    }
                }
                connection.Close();
            }
        }

        public override void Update(CustomerOrder entity)
        {
            var parameters = new SqlParameter[]
            {
            new SqlParameter("@Description", entity.Description),
            new SqlParameter("@CreateDate", entity.CreateDate),
            new SqlParameter("@UpdateDate", entity.UpdateDate),
            new SqlParameter("@SellerId", entity.SellerId),
            new SqlParameter("@CustomerId", entity.CustomerId),
            new SqlParameter("@StatusId", entity.StatusOrder.Id),
            new SqlParameter("@Id", entity.Id)
            };

            ExecuteCommand(UPDATE_QUERY, parameters);
        }
        public void UpdateDetails(OrderDetail detail)
        {
            var parameters = new SqlParameter[] {
        new SqlParameter("@TransportId", detail.TransportId),
        new SqlParameter("@CustomerOrderId", detail.CustomerOrderId),
        new SqlParameter("@TotalAmount", detail.TotalAmount),
        new SqlParameter("@TotalPrice", detail.TotalPrice)
    };
            ExecuteCommand(UPDATE_QUERY_DETAILS, parameters);
        }

        public OrderDetail GetDetails(int productId, int customerOrderId)
        {
            using (var connection = new SqlConnection(con))
            {
                connection.Open();

                var command = new SqlCommand(GET_QUERY_DETAILS, connection);
                command.Parameters.AddWithValue("@CustomerOrderId", customerOrderId);
                command.Parameters.AddWithValue("@TransportId", productId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var transportId = (int)reader["TransportId"];
                        var totalAmount = (int)reader["TotalAmount"];
                        var totalPrice = (decimal)reader["TotalPrice"];
                        var title = (string)reader["Title"];

                        return new OrderDetail
                        {
                            TransportId = transportId,
                            CustomerOrderId = customerOrderId,
                            TotalAmount = totalAmount,
                            TotalPrice = totalPrice,
                            Title = title
                        };
                    }
                }
                return null; 
            }
        }
    }
}
