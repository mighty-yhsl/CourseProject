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
        private const string CREATE_QUERY = "INSERT INTO CustomerOrders(Description, CreateDate, UpdateDate, SellerId, CustomerId, StatusId) OUTPUT INSERTED.ID VALUES (@Description, @CreateDate, @UpdateDate, @SellerId, @CustomerId, @StatusId)";
        private const string CREATE_QUERY_ORDERDETAILS = "INSERT INTO OrderDetails (TotalAmount, TotalPrice, CustomerOrderId, TransportId) VALUES (@TotalAmount, @TotalPrice, @CustomerOrderId, @TransportId)";
        private const string DELETE_QUERY = "DELETE FROM CustomerOrder WHERE Id = @Id";
        private const string DELETE_QUERY_DETAILS = "DELETE FROM OrderDetails WHERE TransportId = @TransportId AND CustomerOrderId = @CustomerOrderId";
        private const string UPDATE_QUERY = "UPDATE CustomerOrders SET Description = @Description, CreateDate = @CreateDate, UpdateDate = @UpdateDate, SellerId = @SellerId, CustomerId = @CustomerId, StatusId = @StatusId WHERE Id = @Id";
        private const string UPDATE_QUERY_DETAILS = "UPDATE OrderDetails SET TotalAmount = @TotalAmount, TotalPrice = @TotalPrice, CustomerOrderId = @CustomerOrderId, TransportId = @TransportId";
        private const string GET_BY_ID_QUERY = "SELECT co.Id, co.Description, co.CreateDate, co.UpdateDate, co.SellerId, co.CustomerId, co.StatusId, od.Id AS OrderDetail_Id, od.TotalAmount, od.TotalPrice, od.CustomerOrderId, od.TransportId, so.Id AS Status_Id, so.Name FROM CustomerOrders co LEFT JOIN OrderDetails od ON co.Id = od.CustomerOrderId LEFT JOIN StatusOrders so ON co.StatusId = so.Id LEFT JOIN Transports t ON od.TransportId = t.Id WHERE co.Id = @Id";
        private const string GET_QUERY = "SELECT co.Id, co.Description, co.CreateDate, co.UpdateDate, co.SellerId, co.CustomerId, co.StatusId, od.Id AS OrderDetailId, od.TotalAmount, od.TotalPrice, od.TransportId FROM CustomerOrders co INNER JOIN OrderDetails od ON co.Id = od.CustomerOrderId";

        
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public OrderRepository() { }

        public override void Create(CustomerOrder entity)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@OperatorId", entity.Description),
                new SqlParameter("@OrderStatusId", entity.CreateDate),
                new SqlParameter("@OrderDate", entity.UpdateDate),
                new SqlParameter("@CustomerName", entity.SellerId),
                new SqlParameter("@CustomerName", entity.CustomerId),
                new SqlParameter("@CustomerName", entity.StatusId)
            };
            var customerOrderId = (int)ExecuteScalarCommand(CREATE_QUERY, parameters);
            foreach (var detail in entity.OrderDetails)
            {
                detail.CustomerOrderId = customerOrderId;
                CreateDetails(detail);
            }
        }

        public void CreateDetails(OrderDetail order)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@TotalAmount", order.TotalAmount),
                new SqlParameter("@TotalPrice", order.TotalPrice),
                new SqlParameter("@CustomerOrderId", order.CustomerOrderId),
                new SqlParameter("@TransportId", order.TransportId)
            };
            ExecuteScalarCommand(CREATE_QUERY_ORDERDETAILS, parameters);
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
            };
            ExecuteCommand(UPDATE_QUERY, parameters);

        }

         public void UpdateDetails(OrderDetail detail)
         {
            var parameters = new SqlParameter[] {
            new SqlParameter("@Description", detail.TotalAmount),
            new SqlParameter("@CreateDate", detail.TotalPrice),
            new SqlParameter("@UpdateDate", detail.CustomerOrderId),
            new SqlParameter("@SellerId", detail.TransportId),
            };
            ExecuteCommand(UPDATE_QUERY_DETAILS, parameters);
         }
    }
}
