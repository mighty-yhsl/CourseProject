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
            if (transport == null || transport.Name.IsNullOrEmpty())
            {
                throw new ArgumentException("Transport is null or fields of Transport are null");
            }

            if (transport.Price < 0 || transport.Price > 100000)
            {
                throw new ArgumentException("Price should be between 0 and 50000");
            }

            if (transport.Amount < 1 || transport.Amount > 50)
            {
                throw new ArgumentException("Amount should be between 1 and 50");
            }

            if (transport.Speed < 1 || transport.Speed > 100)
            {
                throw new ArgumentException("Speed should be between 1 and 100");
            }

            if (transport.Weightt < 1 || transport.Weightt > 50)
            {
                throw new ArgumentException("Weight should be between 1 and 50");
            }

            if (transport.EnginePower < 1 || transport.EnginePower > 1000)
            {
                throw new ArgumentException("EnginePower should be between 1 and 1000");
            }

            if (transport.Name.Length < 1 || transport.Name.Length > 64)
            {
                throw new ArgumentException("Name length should be between 1 and 64");
            }

   
        }

        public void Validate(Manufacturer manufacturer)
        {
            if (manufacturer == null || manufacturer.ManufacturerName.IsNullOrEmpty())
            {
                throw new ArgumentException("Manufacturer is null or ManufacturerName is null or empty");
            }

            if (manufacturer.ManufacturerName.Length < 1 || manufacturer.ManufacturerName.Length > 40)
            {
                throw new ArgumentException("ManufacturerName length should be between 1 and 40");
            }
        }

        public void Validate(Category category)
        {
            if (category == null || category.CategoryName.IsNullOrEmpty())
            {
                throw new ArgumentException("Category is null or CategoryName is null or empty");
            }

            if (category.CategoryName.Length < 1 || category.CategoryName.Length > 40)
            {
                throw new ArgumentException("CategoryName length should be between 1 and 40");
            }
        }
    }
}
