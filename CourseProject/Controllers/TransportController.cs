using CourseProject.BLL.Repositories;
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
    }
}
