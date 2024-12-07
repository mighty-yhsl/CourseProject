using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseProject.DAL.Models.EF;
using CourseProject.BLL.Services;


namespace ProductServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly TransportService _transportService;

        public ProductController(TransportService transportService)
        {
            _transportService = transportService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var transports = _transportService.Get();
            return Ok(transports);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var transport = _transportService.Get(id);
                return Ok(transport);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("by-name/{name}")]
        public IActionResult GetByName(string name)
        {
            try
            {
                var transport = _transportService.GetTransportName(name);
                return Ok(transport);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Transport transport)
        {
            try
            {
                _transportService.CreateTransport(transport);
                return CreatedAtAction(nameof(GetById), new { id = transport.Id }, transport);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Transport transport)
        {
            if (id != transport.Id)
            {
                return BadRequest("ID in route and body do not match.");
            }

            try
            {
                _transportService.UpdateTransport(transport);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _transportService.DeleteTransport(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}