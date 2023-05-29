using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Repositories;
using CourseProject.BLL.Validators;

namespace CourseProject.BLL.Services
{
    public class CustomerService
    {
        private CustomerValidator _customerValidator;
        private CustomerRepository _customerRepository;

        public CustomerService(CustomerRepository customerRepository, CustomerValidator customerValidator)
        {
            _customerValidator = customerValidator;
            _customerRepository = customerRepository;
        }

        public void CreateCustomer(Customer customer)
        {
            try
            {
                _customerValidator.Validate(customer);
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
                _customerValidator.Validate(customer);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Customer not Updated");
            }
            _customerRepository.Update(customer);
        }

        public void DeleteCustomer(int id)
        {
            var customer = _customerRepository.Get(id);
            if (customer is null)
                throw new InvalidOperationException($"Customer with id {id} is not found");
            _customerRepository.Delete(customer);
        }

        public IEnumerable<Customer> Get()
        {
            return _customerRepository.Get();
        }

        public Customer Get(int id)
        {
            return _customerRepository.Get(id);
        }

    }
}
