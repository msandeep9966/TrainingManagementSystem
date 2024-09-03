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

}