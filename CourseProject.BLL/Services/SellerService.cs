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
using CourseProject.BLL.Validators;

namespace CourseProject.BLL.Services
{
    public class SellerService
    {
        private SellerValidator _sellerValidator;
        private SellerRepository _sellerRepository;

        public SellerService(SellerRepository sellerRepository, SellerValidator sellerValidator)
        {
            _sellerValidator = sellerValidator;
            _sellerRepository = sellerRepository;
        }

        public void CreateSeller(Seller seller)
        {
            try
            {
                _sellerValidator.Validate(seller);
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
                _sellerValidator.Validate(seller);
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
        
        public IEnumerable<Seller> Get()
        {
            return _sellerRepository.Get();
        }

        public Seller Get(int id)
        {
            return _sellerRepository.Get(id);
        }
    }
}
