using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Repositories;
using CourseProject.BLL.Repositories;

namespace CourseProject.BLL.Services
{
    public class SellerService
    {
        private SellerRepository _sellerRepository;

        public SellerService(SellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        public void CreateSeller(Seller seller)
        {
            try
            {
                Validate(seller);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Seller not Created");
            }

            _sellerRepository.Create(seller);
        }

        public void UpdateSeller(Seller seller)
        {
            try
            {
                Validate(seller);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Seller not Updated");
            }
            _sellerRepository.Update(seller);
        }

        public void DeleteSeller(int id)
        {
            var seller = _sellerRepository.Get(id);
            if (seller is null)
                throw new InvalidOperationException($"Seller with id {id} is not found");
            _sellerRepository.Delete(seller);
        }

        private void Validate(Seller seller)
        {
            if (seller is null || seller.SellerName.IsNullOrEmpty() || seller.SellerSurname.IsNullOrEmpty() ||
                seller.Phone.IsNullOrEmpty() || seller.Email.IsNullOrEmpty())
            {
                throw new ArgumentException("Seller is null or fields of Seller is null");
            }
            if (seller.SellerSurname.Length > 40)
            {
                throw new ArgumentException("SellerSurname was greater then max length value");
            }
            if (seller.SellerName.Length > 40)
            {
                throw new ArgumentException("SellerName was greater then max length value");
            }
            if (seller.Phone.Length > 40)
            {
                throw new ArgumentException("Phone was greater then max length value");
            }
            if (seller.Email.Length > 40)
            {
                throw new ArgumentException("Email was greater then max length value");
            }
        }
    }
}
