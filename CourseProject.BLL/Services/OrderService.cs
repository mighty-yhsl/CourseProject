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

        public OrderService(OrderRepository orderRepository, OrderValidator orderValidator)
        {
            _orderValidator = orderValidator;
            _orderRepository = orderRepository;
        }
        
        public void CreateOrder(CustomerOrder order)
        {
            try
            {
                _orderValidator.Validate(order);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Order not Created");
            }

            _orderRepository.Create(order);
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

        public void UpdateOrder(CustomerOrder order)
        {
            try
            {
                _orderValidator.Validate(order);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Order not Updated");
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

        /*public void DeleteDetails(int TransportId, int CustomerOrderId)
        {
            var detail = _orderRepository.Get(id);
            if (detail is null)
                throw new InvalidOperationException($"OrderDetail with id {id} is not found");
            _orderRepository.DeleteDetails(detail);
        }*/

      
    }
}
