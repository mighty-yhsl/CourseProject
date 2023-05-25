using CourseProject.DAL.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Interfaces
{
    public interface ISellerRepository
    {
        public Seller GetSeller(int id);

        public IEnumerable<Seller> GetSellers();

        public void Update(Seller seller);

        public void Delete(Seller seller);

        public void Create(Seller seller);
    }
}
