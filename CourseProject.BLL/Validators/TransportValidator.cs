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
            if (transport.Price > 50000)
            {
                throw new ArgumentException("Price should be less 50000");
            }
            if (transport.Amount < 1)
            {
                throw new ArgumentException("Amount should be greater or equals 1");
            }
            if (transport.Amount > 50)
            {
                throw new ArgumentException("Amount should be less 50");
            }
            if (transport.Speed < 1)
            {
                throw new ArgumentException("Speed should be greater or equals 1");
            }
            if (transport.Speed > 70)
            {
                throw new ArgumentException("Speed should be less 70");
            }
            if (transport.Weightt < 1)
            {
                throw new ArgumentException("Speed should be greater or equals 1");
            }
            if (transport.Weightt > 50)
            {
                throw new ArgumentException("Speed should be less 50");
            }
            if (transport.EnginePower < 1)
            {
                throw new ArgumentException("EnginePower should be greater or equals 1");
            }
            if (transport.EnginePower > 1000)
            {
                throw new ArgumentException("EnginePower should be less 1000");
            }
            if (transport.Name.Length > 64)
            {
                throw new ArgumentException("Name should be less 64");
            }
            if (transport.Name.Length < 1)
            {
                throw new ArgumentException("Name should be greater or equals 1");
            }
            if (transport.Id > 0)
            {
                throw new ArgumentException("Id should be greater  0");
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
            if (manufacturer.ManufacturerName.Length < 1)
            {
                throw new ArgumentException("ManufacturerName should be greater or equals 1");
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
            if (category.CategoryName.Length < 1)
            {
                throw new ArgumentException("CategoryName should be greater or equals 1");
            }
        }
    }
}
