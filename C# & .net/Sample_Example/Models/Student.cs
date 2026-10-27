namespace Sample_Example.Models;

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public bool IsActive { get; set; }
}
