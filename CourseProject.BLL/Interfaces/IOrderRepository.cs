using CourseProject.DAL.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Interfaces
{
    public interface IOrderRepository
    {
        public CustomerOrder GetOrder(int id);

        public IEnumerable<CustomerOrder> GetOrders();

        public void Create(CustomerOrder order, List<OrderDetail> orderDetails);

        public void Update(CustomerOrder order);

        public void UpdateDetails(OrderDetail detail);

        public void CreateDetails(OrderDetail detail);

        public void Delete(int id);

        public void DeleteDetails(int id);
    }
}
