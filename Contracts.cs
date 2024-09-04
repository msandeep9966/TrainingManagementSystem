namespace TrainingManagementSystem
{
    public class Contracts
    {
    }

    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CancelCourse
    {
        public int EmployeeId { get; set; }
        public int EnrollmentId { get; set; }
    }

    public class Approval
    {
        public int EmployeeId { get; set; }

        public string? RejectionReason { get; set; }
        public int ManagerId { get; set; }

        public bool Accepted { get; set; }

        public int CourseId { get; set; }
    }

    class EnrollCourseResponse
    {
        public string Message { get; set; }

        public bool Success { get; set; }

    }

}