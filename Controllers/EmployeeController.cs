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
                                     ce.Status,
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
                .FirstOrDefault(t => t.EmployeeId == c.EmployeeId && t.EnrollmentId == c.EnrollmentId && t.Status != "Completed");

            if (deletedCourse != null)
            {
                _dc.CourseEnrollments.Remove(deletedCourse);
                _dc.SaveChanges(); // Save changes to the database
            }

            return deletedCourse == null ? NotFound("Course not found") : Ok(deletedCourse);

        }


        [HttpGet]
        [Route("getpendingcourse")]

        public IActionResult GetPendingCourse([FromQuery] int empid)
        {
            // Retrieve the course enrollment with the given ManagerId and status Pending
            var enrollment = _dc.CourseEnrollments
                             .Join(
                                 _dc.Courses,
                                 ce => ce.CourseId,  // The CourseId from CourseEnrollment
                                 c => c.CourseId,    // The CourseId from Course
                                 (ce, c) => new
                                 {
                                     ce.EnrollmentId,
                                     ce.EmployeeId,
                                     ce.Status,
                                     ce.CourseId,
                                     CourseDetails = c // or select specific properties from `c`
                                 }
                             ).Where(ce => ce.EmployeeId == empid && ce.Status == "Pending").ToList();

            if (enrollment == null)
            {
                return NotFound("No pending enrollment found for the given manager.");
            }


            return Ok(enrollment);
        }

        [HttpGet]
        [Route("getcompletedcourse")]
        public IActionResult GetCompletedCourses([FromQuery] int empid)
        {
            var completedCourses = _dc.CourseEnrollments
                             .Join(
                                 _dc.Courses,
                                 ce => ce.CourseId,  // The CourseId from CourseEnrollment
                                 c => c.CourseId,    // The CourseId from Course
                                 (ce, c) => new
                                 {
                                     ce.EnrollmentId,
                                     ce.EmployeeId,
                                     ce.Status,
                                     ce.CourseId,
                                     CourseDetails = c // or select specific properties from `c`
                                 }
                             ).Where(ce => ce.EmployeeId == empid && ce.Status == "Completed").ToList();

            return completedCourses == null ? NotFound("No courses found") : Ok(completedCourses);
        }

        [HttpGet]
        [Route("getactivecourse")]
        public IActionResult GetActiveCourses([FromQuery] int empid)
        {
            var activeCourses = _dc.CourseEnrollments
                             .Join(
                                 _dc.Courses,
                                 ce => ce.CourseId,  // The CourseId from CourseEnrollment
                                 c => c.CourseId,    // The CourseId from Course
                                 (ce, c) => new
                                 {
                                     ce.EnrollmentId,
                                     ce.EmployeeId,
                                     ce.Status,
                                     ce.CourseId,
                                     CourseDetails = c // or select specific properties from `c`
                                 }
                             ).Where(ce => ce.EmployeeId == empid && ce.Status == "Approved").ToList(); ;

            return activeCourses == null ? NotFound("No courses found") : Ok(activeCourses);
        }

        [HttpPut]
        [Route("completecourse")]
        public IActionResult CompleteCourse([FromQuery] int empid, [FromQuery] int enrollMentId)
        {
            var completedCourse = _dc.CourseEnrollments
                    .FirstOrDefault(t => t.EmployeeId == empid && t.EnrollmentId == enrollMentId && t.Status == "Approved");

            if (completedCourse == null)
            {
                return NotFound("There is no cousrse with Approved status to complete");
            }

            // Update the status based on the 'accept' parameter
            completedCourse.Status = "Completed";
            _dc.Update(completedCourse);

            _dc.SaveChanges();
            return Ok();


        }


        [Route("enrollcourse")]
        [HttpPost]
        public IActionResult EnrollCourse([FromBody] CourseEnrollment[] c)
        {
            foreach (var course in c)
            {
                _dc.CourseEnrollments.Add(course);
            }
            try
            {
                _dc.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                // if the inneerException is there then it will print that .
                // it is like && in react (jsx)
                return NotFound(e.InnerException?.Message ?? e.Message);
            }

        }



    }
}