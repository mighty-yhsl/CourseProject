using CourseProject.BLL.Repositories;
using CourseProject.BLL.Services;
using CourseProject.DAL.Models.EF;
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

        public ActionResult GetTransportName([FromServices] TransportRepository transportRepository, Transport entity) 
        {
            var transport = new Transport { Name = entity.Name };
            var result = transportRepository.GetTransportName(transport);

            if (result == null)
            {
                // Обработка случая, когда объект Transport не найден
                return NotFound();
            }

            var transportList = new List<Transport> { result };
            return View("GetTransportName", transportList);
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
            if (transport is null)
            { 
                 return Redirect("/transport/GetAllTransports");
            }
            return View(transport);
        }

        public ActionResult TryUpdate([FromServices] TransportService transportService, Transport model)
        {
            try
            {
                transportService.UpdateTransport(model);
                return Redirect("/transport/GetAllTransports");
            }
            catch (ArgumentException ex)
            {
                ViewData["Exception"] = ex.Message;
                return View("Update");
            }
        }
    }
}
