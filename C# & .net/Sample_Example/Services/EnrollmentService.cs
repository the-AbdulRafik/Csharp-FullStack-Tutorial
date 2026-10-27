using Sample_Example.Models;
using Sample_Example.Repositories;

namespace Sample_Example.Services;

public class EnrollmentService(EnrollmentRepository repository)
{
    public void Enroll(int studentId, int courseId) => repository.Enroll(studentId, courseId);
    public List<Enrollment> GetAll() => repository.GetAll();
    public void Complete(int enrollmentId) => repository.UpdateStatus(enrollmentId, "Completed");
    public void Cancel(int enrollmentId) => repository.UpdateStatus(enrollmentId, "Cancelled");
}
