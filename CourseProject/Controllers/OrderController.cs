using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using CourseProject.BLL.Services;
using CourseProject.DAL.Models;
using CourseProject.BLL.Repositories;
using CourseProject.ViewModels;
using CourseProject.DAL.Models.EF;

namespace CourseProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService customerOrderService;
        private readonly TransportService transportService;

        public OrderController(OrderService customerOrderService, TransportService transportService)
        {
            this.customerOrderService = customerOrderService;
            this.transportService = transportService;
        }

        public ActionResult Complete()
        {
            return View();
        }

        public ActionResult Details([FromServices] SellerService sellerService, [FromServices] OrderService customerOrderService, int id )
        {
            var order = customerOrderService.Get(id);
            var seller_ = sellerService.Get(id);
            if ((order.SellerId == seller_.Id))
            {
                var seller = sellerService.Get(order.SellerId);
                ViewData["seller"] = $"{seller.SellerName} {seller.SellerSurname}";
            }
            return View(order);
        }

        public ActionResult GetAllOrders([FromServices] OrderRepository orderRepository)
        {
            return View(orderRepository.Get());
        }

        public ActionResult Edit(int id)
        {
            var order = customerOrderService.Get(id);
            OrderVM model = new OrderVM
            {
                Id = order.Id,
                Items = order.OrderDetails.Select(s =>
                    new CartVM
                    {
                        Transport = transportService.Get(s.TransportId),
                        Count = s.TotalAmount,
                    }),
                CustomerName = order.Customer?.CustomerName ?? "",
                Addres = order.Customer?.Addres ?? "",
                Email = order.Customer?.Email ?? "",
                Phone = order.Customer?.Phone?? "",
                Description = order.Description ?? "",
            };
            ViewBag.Transport = new SelectList(transportService.Get().Where(s => model.Items.All(p => p.Transport.Id != s.Id)), "Id", "Title");
            return View(model);
        }

        public ActionResult TryEdit([FromServices] OrderService orderService, OrderVM model)
        {
            var order = orderService.Get(model.Id);

            order.Customer.CustomerName = model.CustomerName;
            order.Customer.Addres = model.Addres;
            order.Customer.Email = model.Email;
            order.Customer.Phone = model.Phone;
            order.Description = model.Description;

            orderService.Update(order);

            return RedirectToAction(controllerName: "Order", actionName: "Details");
        }

        public ActionResult Create([FromServices] CustomerService customerService)
        {
            var model = new OrderVM()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now.AddDays(7)
            }; 
            var cartItemsJson = HttpContext.Session.GetString("cart");

            if (!string.IsNullOrEmpty(cartItemsJson))
            {
                var cartItems = JsonSerializer.Deserialize<List<CartVM>>(cartItemsJson);
                model.Items = cartItems;
            }
            else
            {
                Console.WriteLine("cart is null or empty!");
            }
            
            return View(model);
        }

        public ActionResult TryCreate([FromServices] OrderService orderService, OrderVM model)
        {
            model.Items = JsonSerializer.Deserialize<List<CartVM>>(HttpContext.Session.GetString("cart"));

           
            var customer = new Customer
            {
                Id = model.Id,
            };

            var order = new CustomerOrder
            {
                Customer = customer,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                StatusId = 1,
                Description = model.Description,
                SellerId = 1,
                OrderDetails = model.Items.Select(s => new OrderDetail
                {
                    Id = model.Id,
                    TransportId = s.Transport.Id,
                    TotalAmount = s.Count,
                    TotalPrice = s.Count * s.Transport.Price,
                    Title = s.Transport.Name
                }).ToList()
            };

            orderService.CreateOrder(order);

            HttpContext.Session.Clear();

            return RedirectToAction("Complete", "Order");
        }

        public ActionResult Decrease([FromServices] OrderService orderService, int transportId, int customerOrderId)
        {
            var orderItem = orderService.GetDetails(transportId, customerOrderId);
            if (orderItem.TotalAmount != 1)
            {
                orderItem.TotalAmount -= 1;
                orderService.UpdateDetails(orderItem);
            }
            return RedirectToAction(controllerName: "Order", actionName: "Edit");
        }

        public ActionResult Increase([FromServices] OrderService orderService, int transportId, int customerOrderId)
        {
            var orderItem = orderService.GetDetails(transportId, customerOrderId);
            orderItem.TotalAmount += 1;
            orderService.UpdateDetails(orderItem);
            return RedirectToAction(controllerName: "Order", actionName: "Edit");
        }

        public ActionResult DeleteItem([FromServices] OrderService orderService, int transportId, int customerOrderId)
        {
            orderService.DeleteDetails(customerOrderId, transportId);
            return RedirectToAction(controllerName: "Order", actionName: "Edit");
        }

        public ActionResult AddItem([FromServices] OrderService orderService, int transportId, int customerOrderId)
        {
            orderService.AddDetails(transportId, customerOrderId);
            return RedirectToAction(controllerName: "Order", actionName: "Edit");
        }

        public ActionResult Confirm([FromServices] OrderService orderService,int id)
        {
            var customerOrder = orderService.Get(id);
            customerOrder.StatusOrder.Id = 3;
            orderService.Update(customerOrder);
            return RedirectToAction(controllerName: "Order", actionName: "GetAllOrders");
        }

        public ActionResult Decline([FromServices] OrderService orderService, int id)
        {
            var customerOrder = orderService.Get(id);
            customerOrder.StatusOrder.Id = 4;
            customerOrder.Seller.Id = Convert.ToInt32(HttpContext.User.Claims.First(s => s.Type == "Id").Value);
            orderService.Update(customerOrder);
            return RedirectToAction(controllerName: "Order", actionName: "GetAllOrders");
        }
    }
}