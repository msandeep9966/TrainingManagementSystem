﻿using Microsoft.AspNetCore.Mvc;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Controllers.Manager
{

    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly TMSdbContext _context;
        public ManagerController(TMSdbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("getemployees")]
        public ActionResult GetEmployees(int id)
        {
            var employees = (from e in _context.Employees
                             join m in _context.Managers on e.ManagerId equals m.ManagerId
                             where e.ManagerId == id
                             select new
                             {
                                 e.EmployeeId,
                                 EmployeeName = e.UserName,
                                 EmployeeEmail = e.Email,
                                 ManagerName = m.UserName,
                                 ManagerID = e.ManagerId
                             }
                             ).ToList();


            return Ok(employees);
        }
        [HttpGet]
        [Route("getempcourses")]
        public ActionResult GetEmployeesCourses(int id)
        {

            var empCourses = (from e in _context.Employees
                              join ce in _context.CourseEnrollments
                              on e.EmployeeId equals ce.EmployeeId
                              join c in _context.Courses
                              on ce.CourseId equals c.CourseId
                              join m in _context.Managers
                              on e.ManagerId equals m.ManagerId
                              where m.ManagerId == id
                              select new
                              {
                                  EmployeeName = e.UserName,
                                  CourseName = c.CourseName,
                                  Status = ce.Status,

                              }).ToList();
            return Ok(empCourses);
        }
        //[Flags]
        //public enum EnrollmentStatus
        //{
        //    Pending,
        //    Approved, 
        //    Rejected,
        //    Cancelled,
        //    Completed
        //    
        //}

        [HttpPut]
        [Route("approvecourse")]

        public IActionResult ApproveCourse(Approval aprv)
        {
            // Retrieve the course enrollment with the given ManagerId and status Pending
            var enrollment = _context.CourseEnrollments
                .Where(t => t.ManagerId == aprv.ManagerId && t.Status == "Pending")
                .FirstOrDefault();

            if (enrollment == null)
            {
                return NotFound("No pending enrollment found for the given manager.");
            }

            // Update the status based on the 'accept' parameter
            enrollment.Status = aprv.Accepted ? "Approved" : "Rejected";


            _context.Update(enrollment);


            return Ok($"{_context.SaveChanges()} rows effected");
        }


    }

}