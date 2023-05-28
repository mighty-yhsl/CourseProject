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
    public class CustomerRepository : ICustomerRepository
    {
        private ShopContext _shopContext;

        public CustomerRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public void Create(Customer customer)
        {
            _shopContext.Database.ExecuteSqlRaw("INSERT INTO Customer(CustomerName,CustomerSurname,Phone,Email," +
                "Addres) Values" +
                "(@0,@1,@2,@3,@4,@5)",
                new SqlParameter("@0", customer.CustomerName),
                new SqlParameter("@1", customer.CustomerSurname),
                new SqlParameter("@2", customer.Phone),
                new SqlParameter("@3", customer.Email),
                new SqlParameter("@4", customer.Addres));
        }

        public void Delete(Customer customer)
        {
            _shopContext.Database.ExecuteSqlRaw("DELETE FROM Customer WHERE Id = @0", customer.Id);
        }

        public Customer GetCustomer(int id)
        {
            return _shopContext.Customers.FromSqlRaw("SELECT * FROM Customer WHERE Id = @0", +
                id).FirstOrDefault();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _shopContext.Customers.FromSqlRaw("SELECT * FROM Customer").AsEnumerable();
        }

        public void Update(Customer customer)
        {
            _shopContext.Database.ExecuteSqlRaw("UPDATE Customer SET CustomerName = @0, CustomerSurname = @1, Phone = @2, Email = @3," +
                 "Addres = @4  WHERE Id = @5",
                 new SqlParameter("@0", customer.CustomerName),
                 new SqlParameter("@1", customer.CustomerSurname),
                 new SqlParameter("@2", customer.Phone),
                 new SqlParameter("@3", customer.Email),
                 new SqlParameter("@4", customer.Addres),
                 new SqlParameter("@5", customer.Id));
            //_shopContext.SaveChanges();
        }
    }
}
