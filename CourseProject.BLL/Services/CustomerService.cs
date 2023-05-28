using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Repositories;

namespace CourseProject.BLL.Services
{
    public class CustomerService
    {
        private ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void CreateCustomer(Customer customer)
        {
            try
            {
                Validate(customer);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Customer not Created");
            }

            _customerRepository.Create(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            try
            {
                Validate(customer);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Customer not Updated");
            }
            _customerRepository.Update(customer);
        }

        public void DeleteCustomer(int id)
        {
            var customer = _customerRepository.GetCustomer(id);
            if (customer is null)
                throw new InvalidOperationException($"Customer with id {id} is not found");
            _customerRepository.Delete(customer);
        }

        private void Validate(Customer customer)
        {
            if (customer is null || customer.CustomerName.IsNullOrEmpty() || customer.CustomerSurname.IsNullOrEmpty() ||
                customer.Phone.IsNullOrEmpty() || customer.Email.IsNullOrEmpty())
            {
                throw new ArgumentException("Customer is null or fields of Customer is null");
            }
            if (customer.CustomerSurname.Length > 40)
            {
                throw new ArgumentException("CustomerSurname was greater then max length value");
            }
            if (customer.CustomerName.Length > 40)
            {
                throw new ArgumentException("CustomerName was greater then max length value");
            }
            if (customer.Phone.Length > 40)
            {
                throw new ArgumentException("Phone was greater then max length value");
            }
            if (customer.Email.Length > 40)
            {
                throw new ArgumentException("Email was greater then max length value");
            }
        }
    }
}
