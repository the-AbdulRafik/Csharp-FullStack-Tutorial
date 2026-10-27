using Sample_Example.Models;
using Sample_Example.Repositories;

namespace Sample_Example.Services;

public class DepartmentService(DepartmentRepository repository)
{
    public void Add(string name, string? location) =>
        repository.Add(new Department { DepartmentName = name, Location = location });
    public List<Department> GetAll() => repository.GetAll(true);
    public void Deactivate(int id) => repository.Deactivate(id);
}
