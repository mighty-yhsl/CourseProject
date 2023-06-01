using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.BLL.Repositories;
using CourseProject.DAL.Models.EF;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject.BLL.Validators
{
    public class SellerValidator
    {
        public void Validate(Seller seller)
        {
            if (seller == null || string.IsNullOrEmpty(seller.SellerName) || string.IsNullOrEmpty(seller.SellerSurname) ||
                string.IsNullOrEmpty(seller.Phone) || string.IsNullOrEmpty(seller.Email))
            {
                throw new ArgumentException("Seller is null or fields of Seller are null or empty");
            }

            if (seller.SellerSurname.Length < 1 || seller.SellerSurname.Length > 40)
            {
                throw new ArgumentException("SellerSurname length should be between 1 and 40 characters");
            }

            if (seller.SellerName.Length < 1 || seller.SellerName.Length > 40)
            {
                throw new ArgumentException("SellerName length should be between 1 and 40 characters");
            }

            if (seller.Phone.Length < 1 || seller.Phone.Length > 13)
            {
                throw new ArgumentException("Phone length should be between 1 and 13 characters");
            }

            if (seller.Email.Length < 1 || seller.Email.Length > 40)
            {
                throw new ArgumentException("Email length should be between 1 and 40 characters");
            }

        }
    }
}
