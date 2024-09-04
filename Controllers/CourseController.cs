using Microsoft.AspNetCore.Mvc;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly TMSdbContext _dc;
        public CourseController(TMSdbContext dc)
        {
            _dc = dc;
        }

        [Route("getcourses")]
        [HttpGet]
        public IActionResult getcourses()
        {
            try
            {
                return Ok(_dc.Courses.ToList());
            }
            catch (Exception e)
            {
                // if the inneerException is there then it will print that .
                // it is like && in react (jsx)
                return NotFound(e.InnerException?.Message ?? e.Message);
            }
        }

        [Route("addcourse")]
        [HttpPost]
        public string AddCourse(Course c)
        {
            if (_dc.Courses.Where(t => t.CourseName == t.CourseName && (t.StartDate == c.StartDate && t.EndDate == c.EndDate)).Any())
            {
                return "Course already exists";
            }
            _dc.Courses.Add(c);
            try
            {
                return $"{_dc.SaveChanges()} rows effected";
            }
            catch (Exception e)
            {
                // if the inneerException is there then it will print that .
                // it is like && in react (jsx)
                return e.InnerException?.Message ?? e.Message;
            }

        }

    }
}