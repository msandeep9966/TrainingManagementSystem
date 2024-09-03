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

        [HttpPost]
        [Route("loginuser")]
        public IActionResult Get([FromBody] User user)
        {
            // this null datatype now with `?`
            //bool? isEmployee = null;

            var isEmployeeCheck = _context.Employees
                .Any(t => t.Email == user.Email && t.Password == user.Password);

            if (isEmployeeCheck)
            {
                return Ok(isEmployeeCheck);
            }
            else
            {
                var isManagerCheck = _context.Managers
                    .Any(t => t.Email == user.Email && t.Password == user.Password);

                if (isManagerCheck) { return Ok(false); }

            }

            return NotFound("No user found");

        }
    }
}
