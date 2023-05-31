using CourseProject.BLL.Repositories;
using CourseProject.BLL.Services;
using CourseProject.DAL.Models.EF;
using CourseProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace CourseProject.Controllers
{
    public class SellerController:Controller
    {
        public SellerController()
        { 
        
        }

        public ActionResult GetAllSeller([FromServices] SellerRepository sellerRepository)
        {
            return View(sellerRepository.Get());
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult TryCreate([FromServices] SellerService sellerService, SellerVM model)
        {
            try
            {
                sellerService.CreateSeller(model.ConvertToSeller());
                return Redirect("/seller/GetAllSeller");
            }
            catch (ArgumentException ex)
            {
                ViewData["Exception"] = ex.Message;
                return View("Create");
            }
        }

        public ActionResult GetSellerName([FromServices] SellerService sellerService, SellerVM model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.SellerName))
                {
                    Seller seller = sellerService.GetSellerName(model.SellerName);
                    if (seller != null)
                    {
                        var sellerVM = new SellerVM
                        {
                            SellerName = seller.SellerName,
                            SellerSurname = seller.SellerSurname,
                            Phone = seller.Phone,
                            Email = seller.Email
                        };

                        var sellerList = new List<SellerVM> { sellerVM };
                        return View(sellerList);
                    }
                    else
                    {
                        Console.WriteLine("Seller not found !");
                    }
                }
                else
                {
                    Console.WriteLine("SellerName is null or empty!");
                }

                return Redirect("/seller/GetAllSeller");
            }
            catch (ArgumentException ex)
            {
                ViewData["Exception"] = ex.Message;
                return View("/seller/GetAllSeller");
            }
        }

        public ActionResult Delete([FromServices] SellerService sellerService, int id)
        {
            try
            {
                sellerService.DeleteSeller(id);
                return Redirect("/seller/GetAllSeller");
            }
            catch (ArgumentException ex)
            {
                return Redirect("/seller/GetAllSeller");
            }
        }

        public ActionResult Update([FromServices] SellerRepository sellerRepository,
            [FromServices] SellerService sellerService, int id)
        {
            var seller = sellerRepository.Get(id);
            if (seller is null)
            {
                return Redirect("/seller/GetAllSeller");
            }
            return View(seller);
        }

        public ActionResult TryUpdate([FromServices] SellerService sellerService, Seller model)
        {
            try
            {
                sellerService.UpdateSeller(model);
                return Redirect("/seller/GetAllSeller");
            }
            catch (ArgumentException ex)
            {
                ViewData["Exception"] = ex.Message;
                return View("Update");
            }
        }
    }
}
