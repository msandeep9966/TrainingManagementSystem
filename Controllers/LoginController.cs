using Microsoft.AspNetCore.Mvc;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly TMSdbContext _context;
        public LoginController(TMSdbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Get()
        {
            return Ok(_context.Employees.ToList());
        }
    }
}
