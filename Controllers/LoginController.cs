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

            var EmployeeCheck = _context.Employees
                .Where(t => t.Email == user.Email && t.Password == user.Password).Select(t => new { t.EmployeeId, t.UserName, isEmployee = true, t.ManagerId });
            if (EmployeeCheck.Count() > 0)
            {
                return Ok(EmployeeCheck);
            }
            else
            {
                var ManagerCheck = _context.Managers
                    .Where(t => t.Email == user.Email && t.Password == user.Password).Select(t => new { t.ManagerId, t.UserName, isEmployee = false });

                if (ManagerCheck.Count() > 0) { return Ok(ManagerCheck); }

            }
            return NotFound("No user found");

        }
    }
}
