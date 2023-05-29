using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject.BLL.Validators
{
    public class CustomerValidator
    {
        public void Validate(Customer customer)
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