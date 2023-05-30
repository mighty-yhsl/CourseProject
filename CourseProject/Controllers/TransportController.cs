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
                ViewData["Exception"] = ex.Message;
                return View("Create");
            }
        }

        public ActionResult Delete([FromServices] TransportService transportService, int id)
        {
            try
            {
                transportService.DeleteTransport(id);
                return Redirect("/transport/GetAllTransports");
            }
            catch (ArgumentException ex)
            {
                return Redirect("/transport/GetAllTransports");
            }
        }

        public ActionResult Update([FromServices] TransportRepository transportReposiory,
            [FromServices] TransportService transportService, int id)
        {
            var transport = transportReposiory.Get(id);
            if (transportReposiory is null)
            { 
                 return Redirect("/transport/GetAllTransports");
            }
            return View();
        }
    }
}
