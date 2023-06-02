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
    public class CartController : Controller
    {
        public CartController()
        {
                
        }

        public ActionResult Details()
        {
            var cartJson = HttpContext.Session.GetString("cart");
            List<CartVM> cart;
            if (cartJson is not null)
                cart = JsonSerializer.Deserialize<List<CartVM>>(cartJson);
            else
                cart = new List<CartVM>();
            return View(cart);
        }
/*
        public ActionResult Add([FromServices] TransportRepository transportRepository, int id)
        {
            var transport = transportRepository.Get(id);
            if (transport is null)
                return NotFound();
            AddToCart(transport, id);
            return RedirectToAction(controllerName: "transport", actionName: "get");
        }

        public ActionResult AddQuick([FromServices] TransportRepository transportRepository, int id)
        {
            var transport = transportRepository.Get(id);
            if (transport is null)
                return NotFound();
            AddToCart(transport, id);
            return RedirectToAction(controllerName: "cart", actionName: "details");
        }
*/
        public ActionResult Increase([FromServices] TransportRepository transportRepository, int id)
        {
            var transport = transportRepository.Get(id);
            if (transport is null)
                return NotFound();
            List<CartVM> cartTransports;
            var cartRaw = HttpContext.Session.GetString("cart");
            cartTransports = JsonSerializer.Deserialize<List<CartVM>>(cartRaw);
            var existedItem = cartTransports.FirstOrDefault(s => s.Transport.Id == transport.Id);
            existedItem.Count++;
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartTransports));
            HttpContext.Session.SetInt32("cartCount", cartTransports.Count());
            return RedirectToAction(controllerName: "cart", actionName: "details");
        }

        public ActionResult Decrease([FromServices] TransportRepository transportRepository, int id)
        {
            var transport = transportRepository.Get(id);
            if (transport is null)
                return NotFound();
            List<CartVM> cartTransports;
            var cartRaw = HttpContext.Session.GetString("cart");
            cartTransports = JsonSerializer.Deserialize<List<CartVM>>(cartRaw);
            var existedItem = cartTransports.FirstOrDefault(s => s.Transport.Id == transport.Id);
            if (existedItem.Count != 1)
            {
                existedItem.Count--;
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartTransports));
            HttpContext.Session.SetInt32("cartCount", cartTransports.Count());
            return RedirectToAction(controllerName: "cart", actionName: "details");
        }

        public ActionResult Remove([FromServices] TransportRepository transportRepository, int id)
        {
            var transport = transportRepository.Get(id);
            if (transport is null)
                return NotFound();
            List<CartVM> cartTransports;
            var cartRaw = HttpContext.Session.GetString("cart");
            cartTransports = JsonSerializer.Deserialize<List<CartVM>>(cartRaw);
            var existedItem = cartTransports.FirstOrDefault(s => s.Transport.Id == transport.Id);
            cartTransports.Remove(existedItem);
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartTransports));
            HttpContext.Session.SetInt32("cartCount", cartTransports.Count());
            return RedirectToAction(controllerName: "cart", actionName: "details");
        }

        public ActionResult AddMulti([FromServices] TransportRepository transportRepository, int id, int quantity)
        {
            var transport = transportRepository.Get(id);
            if (transport is null)
                return NotFound();
            List<CartVM> cartProducts;
            var cartRaw = HttpContext.Session.GetString("cart");
            if (cartRaw is null)
            {
                cartProducts = new List<CartVM>();
                cartProducts.Add(new CartVM { Transport = transport, Count = quantity });
            }
            else
            {
                cartProducts = JsonSerializer.Deserialize<List<CartVM>>(cartRaw);
                var existedItem = cartProducts.FirstOrDefault(s => s.Transport.Id == transport.Id);
                if (existedItem is null)
                {
                    existedItem = new CartVM
                    {
                        Transport = transport,
                        Count = quantity
                    };
                    cartProducts.Add(existedItem);
                }
                else
                {
                    existedItem.Count += quantity;
                }
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartProducts));
            HttpContext.Session.SetInt32("cartCount", cartProducts.Count());
            return RedirectToAction(controllerName: "transport", actionName: "details");
        }

        public ActionResult AddToCart([FromServices] TransportRepository transportRepository, int id)
        {
            var transport = transportRepository.Get(id);
            if (transport is null)
                return NotFound();

            AddToCart(transport);

            return RedirectToAction("Details", "Cart");
        }

        private void AddToCart(Transport transport)
        {
            var cartProductsJson = HttpContext.Session.GetString("cart");
            List<CartVM> cartProducts;

            if (cartProductsJson is not null)
            {
                cartProducts = JsonSerializer.Deserialize<List<CartVM>>(cartProductsJson);
                var existingItem = cartProducts.FirstOrDefault(item => item.Transport.Id == transport.Id);

                if (existingItem is null)
                {
                    cartProducts.Add(new CartVM { Transport = transport, Count = 1 });
                }
                else
                {
                    existingItem.Count++;
                }
            }
            else
            {
                cartProducts = new List<CartVM> { new CartVM { Transport = transport, Count = 1 } };
            }

            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartProducts));
            HttpContext.Session.SetInt32("cartCount", cartProducts.Count());
        }
    }
}