using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ShopContext _shopContext;

        public OrderRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public void Create(CustomerOrder order, List<OrderDetail> orderDetails)
        {
            string insertOrderSql = @"
        INSERT INTO CustomerOrders (Description, CreateDate, UpdateDate, SellerId, CustomerId, StatusId)
        VALUES (@0, @1, @2, @3, @4, @5);
        SELECT SCOPE_IDENTITY()";

            // Вставка заказа
            int orderId = _shopContext.Database.ExecuteSqlRaw(insertOrderSql,
                new SqlParameter("@0", order.Description),
                new SqlParameter("@1", order.CreateDate),
                new SqlParameter("@2", order.UpdateDate),
                new SqlParameter("@3", order.SellerId),
                new SqlParameter("@4", order.CustomerId),
                new SqlParameter("@5", order.StatusId));
            _shopContext.SaveChanges();
            // Установка сгенерированного идентификатора заказа
            order.Id = orderId;  //detail.CustomerOrderId = order.Id(В цикле,для оптимизации)

            string insertOrderDetailSql = @"
        INSERT INTO OrderDetails (TotalAmount, TotalPrice, CustomerOrderId, TransportId)
        VALUES (@0, @1, @2, @3)";

            // Вставка деталей заказа
            foreach (var detail in orderDetails)
            {
                _shopContext.Database.ExecuteSqlRaw(insertOrderDetailSql,
                    new SqlParameter("@0", detail.TotalAmount),
                    new SqlParameter("@1", detail.TotalPrice),
                    new SqlParameter("@2", order.Id),
                    new SqlParameter("@3", detail.TransportId));
            }
            
          
        }

        public void CreateDetails(OrderDetail detail)
        {
            _shopContext.Database.ExecuteSqlRaw("INSERT INTO OrderDetails(TotalAmount,TotalPrice,CustomerOrderId" +
                ",TransportId) Values" +
               "(@0,@1,@2,@3)",
               new SqlParameter("@0", detail.TotalAmount),
               new SqlParameter("@1", detail.TotalPrice),
               new SqlParameter("@2", detail.CustomerOrderId),
               new SqlParameter("@3", detail.TransportId));
        }

        public void Delete(CustomerOrder order)
        {
            _shopContext.Database.ExecuteSqlRaw("DELETE FROM CustomerOrder WHERE Id = @0", order.Id);
        }
    

        public void DeleteDetails(OrderDetail detail)
        {
            _shopContext.Database.ExecuteSqlRaw("DELETE FROM OrderDetails WHERE Id = @0", detail.Id);
        }

        public OrderDetail GetDetails(int id)
        {
            return _shopContext.OrderDetails.FromSqlRaw("SELECT * FROM OrderDetail WHERE Id = @0", +
               id).FirstOrDefault();
        }

            public CustomerOrder GetOrder(int id)
        {
            return _shopContext.CustomerOrders.FromSqlRaw("SELECT * FROM CustomerOrder WHERE Id = @0", +
               id).FirstOrDefault();

            /*string sql = @"
        SELECT co.Id, co.Description, co.CreateDate, co.UpdateDate, co.SellerId, co.CustomerId, co.StatusId,
               od.Id AS OrderDetail_Id, od.TotalAmount, od.TotalPrice, od.CustomerOrderId, od.TransportId,
               so.Id AS Status_Id, so.Name
        FROM CustomerOrders co
        LEFT JOIN OrderDetails od ON co.Id = od.CustomerOrderId
        LEFT JOIN StatusOrders so ON co.StatusId = so.Id
        LEFT JOIN Transports t ON od.TransportId = t.Id
        WHERE co.Id = @Id";

    var result = _shopContext.Database.ExecuteQuery<CustomerOrder>(sql, new SqlParameter("@Id", id));

    return result.FirstOrDefault();*/
        }

        public IEnumerable<CustomerOrder> GetOrders()
        {
            return _shopContext.CustomerOrders.FromSqlRaw("SELECT * FROM CustomerOrder").AsEnumerable();
        }

        public void Update(CustomerOrder order)
        {
            _shopContext.Database.ExecuteSqlRaw("UPDATE CustomerOrder SET Description = @0, CreateDate = @1," +
                " UpdateDate = @2, SellerId = @3, CustomerId = @4, StatusId = @5 WHERE Id = @6",
                new SqlParameter("@0", order.Description),
                new SqlParameter("@1", order.CreateDate),
                new SqlParameter("@2", order.UpdateDate),
                new SqlParameter("@3", order.SellerId),
                new SqlParameter("@4", order.CustomerId),
                new SqlParameter("@5", order.StatusId),
                new SqlParameter("@6", order.Id));
            //_shopContext.SaveChanges();
        }

        public void UpdateDetails(OrderDetail detail)
        {
            _shopContext.Database.ExecuteSqlRaw("UPDATE OrderDetail SET TotalAmount = @0, TotalPrice = @1, " +
                "CustomerOrderId = @2, TransportId = @3 WHERE Id = @4",
                new SqlParameter("@0", detail.TotalAmount),
                new SqlParameter("@1", detail.TotalPrice),
                new SqlParameter("@2", detail.CustomerOrderId),
                new SqlParameter("@3", detail.TransportId),
                new SqlParameter("@4", detail.Id));
            //_shopContext.SaveChanges();
        }
    }
}
