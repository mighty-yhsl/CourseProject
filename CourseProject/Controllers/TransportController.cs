using CourseProject.BLL.Repositories;
using CourseProject.BLL.Services;
using CourseProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class TransportController:Controller
    {
        public TransportController()
        { 
        }

        public ActionResult GetAllTransports([FromServices] TransportRepository transportRepository) 
        {
            return View(transportRepository.Get());
        }

        public ActionResult Create() 
        {
            return View();
        }

        public ActionResult TryCreate([FromServices] TransportService transportService, TransportVM model)
        {
            try
            {
                transportService.CreateTransport(model.ConvertToTransport());
                return Redirect("GetAllTransports");
            }
            catch (ArgumentException ex) 
            {
                ViewData["Exeption"] = ex.Message;
                return View("Create");
            }
        }
    }
}
