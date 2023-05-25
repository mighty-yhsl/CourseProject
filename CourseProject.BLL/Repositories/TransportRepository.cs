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
    public class TransportRepository : ITransportRepository
    {
        private ShopContext _shopContext;

        public TransportRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public void Create(Transport transport)
        {
            _shopContext.Database.ExecuteSqlRaw("INSERT INTO Transport([Name],Speed,Weightt,EnginePower," +
                "Amount, Price, CategoryId, ManufacturerId) Values" +
                "(@0,@1,@2,@3,@4,@5,@6,@7)",
                new SqlParameter("@0",transport.Name),
                new SqlParameter("@1", transport.Speed),
                new SqlParameter("@2", transport.Weightt),
                new SqlParameter("@3", transport.EnginePower),
                new SqlParameter("@4", transport.Amount),
                new SqlParameter("@5", transport.Price),
                new SqlParameter("@6", transport.CategoryId),
                new SqlParameter("@7", transport.ManufacturerId));
        }

        public void Delete(Transport transport)
        {
            _shopContext.Database.ExecuteSqlRaw("DELETE FROM Transport WHERE Id = @0", transport.Id);
        }

        public Transport GetTransport(int id)
        {
            return _shopContext.Transports.FromSqlRaw("SELECT * FROM Transport WHERE Id = @0", +
                id).FirstOrDefault();
        }

        public IEnumerable<Transport> GetTransports()
        {
            return _shopContext.Transports.FromSqlRaw("SELECT * FROM Transport").AsEnumerable();
        }

        public void Update(Transport transport)
        {
            _shopContext.Database.ExecuteSqlRaw("UPDATE Transport SET [Name] = @0, Speed = @1, Weightt = @2, EnginePower = @3, " +
                "Amount = @4, Price = @5, CategoryId = @6, ManufacturerId = @7 WHERE Id = @8",
                new SqlParameter("@0", transport.Name),
                new SqlParameter("@1", transport.Speed),
                new SqlParameter("@2", transport.Weightt),
                new SqlParameter("@3", transport.EnginePower),
                new SqlParameter("@4", transport.Amount),
                new SqlParameter("@5", transport.Price),
                new SqlParameter("@6", transport.CategoryId),
                new SqlParameter("@7", transport.ManufacturerId),
                new SqlParameter("@8", transport.Id));
        }
    }
}
