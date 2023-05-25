using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private ShopContext _shopContext;

        public CustomerRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public void Create(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void Delete(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public void Update(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
