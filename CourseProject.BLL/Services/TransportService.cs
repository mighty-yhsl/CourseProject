using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
namespace CourseProject.BLL.Services
{
    public class TransportService
    {
        private ITransportRepository _transportRepository;

        public TransportService(ITransportRepository transportRepository)
        {
            _transportRepository = transportRepository;
        }
        public void CreateTransport(Transport transport)
        {
            try
            {
                Validate(transport);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Transport not Created");
            }

            _transportRepository.Create(transport);
        }

        public void UpdateTransport(Transport transport)
        {
            try
            {
                Validate(transport);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Transport not Updated");
            }
            _transportRepository.Update(transport);
        }

        public void DeleteProduct(int id)
        {
            var transport = _transportRepository.GetTransport(id);
            if (transport is null)
                throw new InvalidOperationException($"Transport with id {id} is not found");
            _transportRepository.Delete(transport);
        }

        private void Validate(Transport transport)
        {
            if (transport is null || transport.Name.IsNullOrEmpty())
            {
                throw new ArgumentException("transport is null or fields of transport is null");
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
        }
    }
}
