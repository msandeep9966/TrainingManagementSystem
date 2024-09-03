using Microsoft.AspNetCore.Mvc;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        private readonly TMSdbContext _dc;
        public HelperController(TMSdbContext dc)
        {
            _dc = dc;
        }
        [HttpGet]
        [Route("getempid")]
        public IActionResult GetEmpId([FromQuery] string empname)
        {
            //var res = _dc.Employees.Where(_dc => _dc.UserName == empname.ToLower()).FirstOrDefault();
            var res = _dc.Employees.Where(_dc => _dc.UserName == empname).Select(t => new { t.UserName, t.EmployeeId, t.ManagerId }).FirstOrDefault();
            return res == null ? NotFound("User not found") : Ok(res);
        }

        [HttpGet]
        [Route("getmgrid")]
        public IActionResult GetMgrId(string mgrname)
        {
            //var res = _dc.Employees.Where(_dc => _dc.UserName == empname.ToLower()).FirstOrDefault();
            var res = _dc.Employees.Where(_dc => _dc.UserName == mgrname).FirstOrDefault();
            return res == null ? NotFound("User not found") : Ok(res.ManagerId);
        }
    }
}
