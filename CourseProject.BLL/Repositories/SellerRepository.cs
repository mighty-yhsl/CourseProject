using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private ShopContext _shopContext;

        public SellerRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public void Create(Seller seller)
        {
            _shopContext.Database.ExecuteSqlRaw("INSERT INTO Seller(SellerName,SellerSurname,Phone,Email) Values" +
                 "(@0,@1,@2,@3,@4,@5)",
                 new SqlParameter("@0", seller.SellerName),
                 new SqlParameter("@1", seller.SellerSurname),
                 new SqlParameter("@2", seller.Phone),
                 new SqlParameter("@3", seller.Email));
        }

        public void Delete(Seller seller)
        {
            _shopContext.Database.ExecuteSqlRaw("DELETE FROM Seller WHERE Id = @0", seller.Id);
        }

        public Seller GetSeller(int id)
        {
            return _shopContext.Sellers.FromSqlRaw("SELECT * FROM Seller WHERE Id = @0", +
               id).FirstOrDefault();
        }

        public IEnumerable<Seller> GetSellers()
        {
            return _shopContext.Sellers.FromSqlRaw("SELECT * FROM Seller").AsEnumerable();
        }

        public void Update(Seller seller)
        {
            _shopContext.Database.ExecuteSqlRaw("UPDATE Seller SET SellerName = @0, SellerSurname = @1, Phone = @2, " +
                "Email = @3 WHERE Id = @4",
                 new SqlParameter("@0", seller.SellerName),
                 new SqlParameter("@1", seller.SellerSurname),
                 new SqlParameter("@2", seller.Phone),
                 new SqlParameter("@3", seller.Email),
                 new SqlParameter("@4", seller.Id));
            //_shopContext.SaveChanges();
        }
    }
}
