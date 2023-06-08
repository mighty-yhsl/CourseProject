using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Repositories;
using CourseProject.BLL.Validators;

namespace CourseProject.BLL.Services
{
    public class OrderService
    {
        private OrderValidator _orderValidator;
        private OrderRepository _orderRepository;
        private TransportRepository _transportRepository;

        public OrderService(OrderRepository orderRepository, OrderValidator orderValidator, TransportRepository transportRepository)
        {
            _orderValidator = orderValidator;
            _orderRepository = orderRepository;
            _transportRepository = transportRepository; 
        }
        
        public int CreateOrder(CustomerOrder order)
        {
            try
            {
                _orderValidator.Validate(order);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Order not Created");
            }
            
            return _orderRepository.CreateScalar(order);
        }

        public void CreateDetails(OrderDetail detail)
        {
            try
            {
                _orderValidator.Validate(detail);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"OrderDetail not Created");
            }

            _orderRepository.CreateDetails(detail);
        }



        public void Update(CustomerOrder order)
        {
            if (_orderRepository.Get(order.Id) is null)
                throw new Exception($"CustomerOrder with ID {order.Id} is not found");
            try
            {
                _orderValidator.Validate(order);
            }
            catch (ArgumentException)
            {
                throw;
            }
            _orderRepository.Update(order);
        }

        public void UpdateDetails(OrderDetail detail)
        {
            try
            {
                _orderValidator.Validate(detail);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"OrderDetail not Updated");
            }
            _orderRepository.UpdateDetails(detail);
        }

        public void DeleteOrder(int id)
        {
            var order = _orderRepository.Get(id);
            if (order is null)
                throw new InvalidOperationException($"Замовлення з таким {id} не знайдено");
            _orderRepository.Delete(order);
        }

        public void DeleteDetails(int transportId, int CustomerOrderId)
        {
            _orderRepository.DeleteDetails(transportId, CustomerOrderId);
        }

        public OrderDetail GetDetails(int transportId, int customerOrderId)
        {
            return _orderRepository.GetDetails(transportId, customerOrderId);
        }

        public void UpdateDetail(OrderDetail detail)
        {
            _orderRepository.UpdateDetails(detail);
        }


        public IEnumerable<CustomerOrder> Get()
        {
            return _orderRepository.Get();
        }
        public CustomerOrder Get(int id)
        {
            return _orderRepository.Get(id);
        }

        public void AddDetails(int transportId, int customerOrderId)
        {
            var transport = _transportRepository.Get(transportId);
            _orderRepository.CreateDetails(new OrderDetail
            {
                TotalPrice = transport.Price,
                TransportId = transportId,
                CustomerOrderId = customerOrderId,
                TotalAmount = 1,
                Title = transport.Name
            });
        }
    }
}
