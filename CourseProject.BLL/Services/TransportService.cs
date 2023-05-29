using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using CourseProject.BLL.Interfaces;
using CourseProject.BLL.Repositories;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Validators;
using Azure;

namespace CourseProject.BLL.Services
{
    public class TransportService
    {
        private TransportValidator _transportValidator;
        private TransportRepository _transportRepository;

        public TransportService(TransportRepository transportRepository, TransportValidator transportValidator)
        {
            _transportValidator = transportValidator;
            _transportRepository = transportRepository;
        }
        public void CreateTransport(Transport transport, Category category, Manufacturer manufacturer)
        {
            try
            {
                _transportValidator.Validate(transport);
                _transportValidator.Validate(category);
                _transportValidator.Validate(manufacturer);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Transport not Created");
            }

            _transportRepository.Create(transport);
        }

        public void UpdateTransport(Transport transport, Category category, Manufacturer manufacturer)
        {
            try
            {
                _transportValidator.Validate(transport);
                _transportValidator.Validate(category);
                _transportValidator.Validate(manufacturer);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Transport not Updated");
            }
            _transportRepository.Update(transport);
        }

        public void DeleteTransport(int id)
        {
            var transport = _transportRepository.Get(id);
            if (transport is null)
                throw new InvalidOperationException($"Transport with id {id} is not found");
            _transportRepository.Delete(transport);
        }

        public IEnumerable<Transport> Get()
        {
            return _transportRepository.Get();
        }

        public Transport Get(int id)
        {
            return _transportRepository.Get(id);
        }
    }
}
