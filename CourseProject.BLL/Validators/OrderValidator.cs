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
    public class OrderValidator
    {
        public void Validate(CustomerOrder order)
        {
            if (order is null)
            {
                throw new ArgumentException("Order is null");
            }
            if ((!order.Description.IsNullOrEmpty() && order.Description.Length > 256))
            {
                throw new ArgumentException("Description was greater then max length value or Description is not null");
            }
            
            
        }

        public void Validate(OrderDetail detail)
        {
            if (detail is null)
            {
                throw new ArgumentException("OrderDetail is null");
            }
            if (detail.TotalPrice < 0)
            {
                throw new ArgumentException("TotalPrice should be greater or equals 0");
            }
            if (detail.TotalAmount < 1)
            {
                throw new ArgumentException("TotalAmount should be greater or equals 1");
            }
        }
    }
}
