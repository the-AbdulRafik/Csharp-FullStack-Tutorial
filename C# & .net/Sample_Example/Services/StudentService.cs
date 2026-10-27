using Sample_Example.Models;
using Sample_Example.Repositories;

namespace Sample_Example.Services;

public class StudentService(StudentRepository repository)
{
    public void Add(string name, string email, int age, int departmentId) =>
        repository.Add(new Student { Name = name, Email = email, Age = age, DepartmentId = departmentId });
    public List<Student> Search(string? text = null) => repository.GetAll(true, text);
    public void Deactivate(int id) => repository.Deactivate(id);
}
