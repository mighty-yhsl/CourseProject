using CourseProject.DAL.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Interfaces
{
    public interface ICustomerRepository
    {
        public Customer GetCustomer(int id);

        public IEnumerable<Customer> GetCustomers();

        public void Update(Customer customer);

        public void Delete(Customer customer);

        public void Create(Customer customer);
    }
}
