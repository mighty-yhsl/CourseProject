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

namespace CourseProject.BLL.Repositories
{
    public class OrderRepository : Repository<CustomerOrder>
    {
        private const string CREATE_QUERY_CUSTOMER_ORDER = "INSERT INTO CustomerOrders(Description, CreateDate, UpdateDate, SellerId, CustomerId, StatusId) OUTPUT INSERTED.Id VALUES (@Description, @CreateDate, @UpdateDate, @SellerId, @CustomerId, @StatusId)";
        private const string CREATE_QUERY_CUSTOMER_ORDERDETAILS = "INSERT INTO OrderDetails (TotalAmount, TotalPrice, CustomerOrderId, TransportId) VALUES (@TotalAmount, @TotalPrice, @CustomerOrderId, @TransportId)";
        // private const string CREATE_QUERY_ORDERDETAILS = "";
        private const string DELETE_QUERY = "DELETE FROM CustomerOrder WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE CustomerOrders SET Description = @Description, CreateDate = @CreateDate, UpdateDate = @UpdateDate, SellerId = @SellerId, CustomerId = @CustomerId, StatusId = @StatusId WHERE Id = @Id";
        private const string GET_BY_ID_QUERY = "SELECT co.Id, co.Description, co.CreateDate, co.UpdateDate, co.SellerId, co.CustomerId, co.StatusId, od.Id AS OrderDetail_Id, od.TotalAmount, od.TotalPrice, od.CustomerOrderId, od.TransportId, so.Id AS Status_Id, so.Name FROM CustomerOrders co LEFT JOIN OrderDetails od ON co.Id = od.CustomerOrderId LEFT JOIN StatusOrders so ON co.StatusId = so.Id LEFT JOIN Transports t ON od.TransportId = t.Id WHERE co.Id = @Id";
        private const string GET_QUERY = "SELECT co.Id, co.Description, co.CreateDate, co.UpdateDate, co.SellerId, co.CustomerId, co.StatusId, od.Id AS OrderDetailId, od.TotalAmount, od.TotalPrice, od.TransportId FROM CustomerOrders co INNER JOIN OrderDetails od ON co.Id = od.CustomerOrderId";

        
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public OrderRepository() { }

        public override void Create(CustomerOrder entity)
        {

            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(CREATE_QUERY_CUSTOMER_ORDER, connection))
                {
                    command.Parameters.AddWithValue("@Description", entity.Description);
                    command.Parameters.AddWithValue("@CreateDate", entity.CreateDate);
                    command.Parameters.AddWithValue("@UpdateDate", entity.UpdateDate);
                    command.Parameters.AddWithValue("@SellerId", entity.SellerId);
                    command.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
                    command.Parameters.AddWithValue("@StatusId", entity.StatusId);

                    int orderId = (int)command.ExecuteScalar();
                    entity.Id = orderId;
                }

                foreach (var detail in entity.OrderDetails)
                {
                    using (SqlCommand command = new SqlCommand(CREATE_QUERY_CUSTOMER_ORDERDETAILS, connection))
                    {
                        command.Parameters.AddWithValue("@TotalAmount", detail.TotalAmount);
                        command.Parameters.AddWithValue("@TotalPrice", detail.TotalPrice);
                        command.Parameters.AddWithValue("@CustomerOrderId", entity.Id);
                        command.Parameters.AddWithValue("@TransportId", detail.TransportId);

                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }

        }

        /*public override void Create(OrderDetail entity)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@TotalAmount", entity.TotalAmount),
                new SqlParameter("@TotalPrice", entity.TotalPrice),
                new SqlParameter("@CustomerOrderId", entity.CustomerOrderId),
                new SqlParameter("@TransportId", entity.TransportId)
            };
            ExecuteScalarCommand(CREATE_QUERY_ORDERDETAILS, parameters);
        }*/
        public override void Delete(CustomerOrder entity)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@Id", entity.Id) };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        /*public void DeleteDetails(OrderDetail detail)
        {
            _shopContext.Database.ExecuteSqlRaw("DELETE FROM OrderDetails WHERE Id = @0", detail.Id);
        }

        public OrderDetail GetDetails(int id)
        {
            return _shopContext.OrderDetails.FromSqlRaw("SELECT * FROM OrderDetail WHERE Id = @0", +
               id).FirstOrDefault();
        }*/

        public override CustomerOrder Get(int id)
        {
           CustomerOrder customerorder = new CustomerOrder();
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
                            customerorder.Id = Convert.ToInt32(reader["Id"]);
                            customerorder.Description = Convert.ToString(reader["Description"]);
                            customerorder.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
                            customerorder.UpdateDate = Convert.ToDateTime(reader["UpdateDate"]);
                            customerorder.SellerId = Convert.ToInt32(reader["SellerId"]);
                            customerorder.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                            customerorder.StatusId = Convert.ToInt32(reader["StatusId"]);

                            customerorder.OrderDetails = new List<OrderDetail>
                            {
                                new OrderDetail
                                {
                                    TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                                    TotalPrice = Convert.ToDecimal(reader["TotalPrice"])
                                }
                            };
                        }
                    }
                }
                connection.Close();
            }
            return customerorder;
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
                        int previousOrderId = 0;
                        CustomerOrder customerOrder = null;

                        while (reader.Read())
                        {
                            int orderId = Convert.ToInt32(reader["Id"]);

                            if (orderId != previousOrderId)
                            {
                                customerOrder = new CustomerOrder();
                                customerOrder.Id = orderId;
                                customerOrder.Description = Convert.ToString(reader["Description"]);
                                customerOrder.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
                                customerOrder.UpdateDate = Convert.ToDateTime(reader["UpdateDate"]);
                                customerOrder.SellerId = Convert.ToInt32(reader["SellerId"]);
                                customerOrder.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                                customerOrder.StatusId = Convert.ToInt32(reader["StatusId"]);

                                customerOrder.OrderDetails = new List<OrderDetail>();
                                customerOrders.Add(customerOrder);

                                previousOrderId = orderId;
                            }
                            OrderDetail orderDetail = new OrderDetail();
                            orderDetail.Id = Convert.ToInt32(reader["OrderDetailId"]);
                            orderDetail.TotalAmount = Convert.ToInt32(reader["TotalAmount"]);
                            orderDetail.TotalPrice = Convert.ToDecimal(reader["TotalPrice"]);
                            orderDetail.TransportId = Convert.ToInt32(reader["TransportId"]);

                            customerOrder.OrderDetails.Add(orderDetail);
                        }
                    }
                }
                connection.Close();
            }
            return customerOrders;
        }


        public override void Update(CustomerOrder entity)
        {
            var parameters = new SqlParameter[] {
            new SqlParameter("@Description", entity.Description),
            new SqlParameter("@CreateDate", entity.CreateDate),
            new SqlParameter("@UpdateDate", entity.UpdateDate),
            new SqlParameter("@SellerId", entity.SellerId),
            new SqlParameter("@CustomerId", entity.CustomerId),
            new SqlParameter("@StatusId", entity.StatusId),
            new SqlParameter("@Id", entity.Id),
            };
            ExecuteCommand(UPDATE_QUERY, parameters);

        }

        /* public void UpdateDetails(OrderDetail detail)
         {
             _shopContext.Database.ExecuteSqlRaw("UPDATE OrderDetail SET TotalAmount = @0, TotalPrice = @1, " +
                 "CustomerOrderId = @2, TransportId = @3 WHERE Id = @4",
                 new SqlParameter("@0", detail.TotalAmount),
                 new SqlParameter("@1", detail.TotalPrice),
                 new SqlParameter("@2", detail.CustomerOrderId),
                 new SqlParameter("@3", detail.TransportId),
                 new SqlParameter("@4", detail.Id));
             //_shopContext.SaveChanges();
         }*/
    }
}
