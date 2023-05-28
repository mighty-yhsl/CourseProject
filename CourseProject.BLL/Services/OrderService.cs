using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CourseProject.BLL.Interfaces;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Repositories;

namespace CourseProject.BLL.Services
{
    public class OrderService
    {
        private IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        
        public void CreateOrder(CustomerOrder order, List<OrderDetail> orderDetails)
        {
            try
            {
                ValidateOrder(order);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Order not Created");
            }

            _orderRepository.Create(order,orderDetails);
        }

        public void CreateDetails(OrderDetail detail)
        {
            try
            {
                ValidateDetail(detail);
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
                ValidateOrder(order);
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
                ValidateDetail(detail);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"OrderDetail not Updated");
            }
            _orderRepository.UpdateDetails(detail);
        }

        public void DeleteOrder(int id)
        {
            var order = _orderRepository.GetOrder(id);
            if (order is null)
                throw new InvalidOperationException($"Order with id {id} is not found");
            _orderRepository.Delete(order);
        }

        public void DeleteDetails(int id)
        {
            var detail = _orderRepository.GetDetails(id);
            if (detail is null)
                throw new InvalidOperationException($"OrderDetail with id {id} is not found");
            _orderRepository.DeleteDetails(detail);
        }

        private void ValidateOrder(CustomerOrder order)
        {
            if (order is null)
            {
                throw new ArgumentException("Order is null");
            }
            if ((!order.Description.IsNullOrEmpty() && order.Description.Length > 256))
            {
                throw new ArgumentException("Description was greater then max length value or Description is not null");
            }
            if (order.CreateDate > DateTime.Now)
            {
                throw new ArgumentException("CreateDate cannot be in the future");
            }
        }

        private void ValidateDetail(OrderDetail detail)
        {
            if (detail is null)
            {
                throw new ArgumentException("OrderDetail is null");
            }
            if (detail.TotalPrice < 0)
            {
                throw new ArgumentException("TotalPrice should be greater or equals 0");
            }
            if (detail.TotalAmount < 1)
            {
                throw new ArgumentException("TotalAmount should be greater or equals 1");
            }
        }
    }
}
