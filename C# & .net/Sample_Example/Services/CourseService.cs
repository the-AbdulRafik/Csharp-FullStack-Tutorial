using Sample_Example.Models;
using Sample_Example.Repositories;

namespace Sample_Example.Services;

public class CourseService(CourseRepository repository)
{
    public void Add(string name, int duration, decimal fee, int departmentId) =>
        repository.Add(new Course
        {
            CourseName = name, DurationMonths = duration, Fee = fee, DepartmentId = departmentId
        });
    public List<Course> GetAll() => repository.GetAll(true);
    public void Deactivate(int id) => repository.Deactivate(id);
}
