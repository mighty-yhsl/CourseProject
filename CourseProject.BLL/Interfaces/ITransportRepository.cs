using CourseProject.DAL.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Interfaces
{
    public interface ITransportRepository
    {
        public Transport GetTransport(int id);

        public IEnumerable<Transport> GetTransports();

        public void Update(Transport transport);

        public void Delete(Transport transport);

        public void Create(Transport transport);
    }
}
