using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CourseProject.BLL.Repositories;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Validators;


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
       
        public void CreateTransport(Transport transport)
        {
            try
            {
                _transportValidator.Validate(transport);
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
                _transportValidator.Validate(transport);
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
                throw new InvalidOperationException($"Товар з таким {id} не знайдено");
            _transportRepository.Delete(transport);
        }

        public Transport GetTransportName(string name)
        {
            var transport = _transportRepository.GetTransportName(name);
            if (transport is null)
                throw new InvalidOperationException($"Товар з таким '{name}' не знайдено");
            return transport;
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
