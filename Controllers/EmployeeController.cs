using Microsoft.AspNetCore.Mvc;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly TMSdbContext _dc;
        public EmployeeController(TMSdbContext dc)
        {
            _dc = dc;
        }

        [HttpPost]
        [Route("addcourse")]
        public int AddCourse(Course c)
        {
            _dc.Courses.Add(c);
            return _dc.SaveChanges();
        }

        [HttpPost]
        [Route("enrollcourse")]
        public int EnrollCourse(CourseEnrollment c)
        {
            _dc.CourseEnrollments.Add(c);
            return _dc.SaveChanges();
        }

        [HttpGet]
        [Route("getcoursesbyempid")]
        public IActionResult GetCoursesByEmpId([FromQuery] int empid)
        {

            var courseEnrollments = _dc.CourseEnrollments
                             .Join(
                                 _dc.Courses,
                                 ce => ce.CourseId,  // The CourseId from CourseEnrollment
                                 c => c.CourseId,    // The CourseId from Course
                                 (ce, c) => new
                                 {
                                     ce.EnrollmentId,
                                     ce.EmployeeId,
                                     ce.CourseId,
                                     CourseDetails = c // or select specific properties from `c`
                                 }
                             ).Where(ce => ce.EmployeeId == empid).ToList();

            return courseEnrollments == null ? NotFound("No courses found") : Ok(courseEnrollments);
        }

        [HttpPost]
        [Route("cancelcourse")]
        public IActionResult CancelCourse([FromBody] CancelCourse c)
        {
            var deletedCourse = _dc.CourseEnrollments
                .FirstOrDefault(t => t.EmployeeId == c.EmployeeId && t.EnrollmentId == c.EnrollmentId);

            if (deletedCourse != null)
            {
                _dc.CourseEnrollments.Remove(deletedCourse);
                _dc.SaveChanges(); // Save changes to the database
            }

            return deletedCourse == null ? NotFound("Course not found") : Ok(deletedCourse);

        }

    }
}