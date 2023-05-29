using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.DAL.Models.EF;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject.BLL.Validators
{
    public class TransportValidator
    {
        public void Validate(Transport transport)
        {
            if (transport is null || transport.Name.IsNullOrEmpty())
            {
                throw new ArgumentException("Transport is null or fields of Transport is null");
            }
            if (transport.Price < 0)
            {
                throw new ArgumentException("Price should be greater or equals 0");
            }
            if (transport.Amount < 1)
            {
                throw new ArgumentException("Amount should be greater or equals 1");
            }
            if (transport.Speed < 1 && transport.Weightt < 1 && transport.EnginePower < 1)
            {
                throw new ArgumentException("One of string fields was greater or equals 1");
            }
            if (transport.Name.Length > 64)
            {
                throw new ArgumentException("Name was greater then max length value");
            }
            if (transport is null || transport.Name.IsNullOrEmpty())
            {
                throw new ArgumentException("Transport is null or fields of Transport is null");
            }
        }

        public void Validate(Manufacturer manufacturer) 
        {
            if (manufacturer is null || manufacturer.ManufacturerName.IsNullOrEmpty())
            {
                throw new ArgumentException("Manufacturer is null ");
            }
            if (manufacturer.ManufacturerName.Length > 40)
            {
                throw new ArgumentException("ManufacturerName was greater then max length value");
            }
        }

        public void Validate(Category category)
        {
            if (category is null || category.CategoryName.IsNullOrEmpty())
            {
                throw new ArgumentException("Category is null ");
            }
            if(category.CategoryName.Length > 40)
            {
                throw new ArgumentException("CategoryName was greater then max length value");
            }
        }
    }
}
