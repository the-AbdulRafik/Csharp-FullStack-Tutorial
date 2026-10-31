using Microsoft.AspNetCore.Mvc;

using WebApiPractice.Models;
using WebApiPractice.Services;
using WebApiPractice.DTOs;
namespace WebApiPractice.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }

    // GET: api/students
    [HttpGet]
    public ActionResult<List<Student>> GetAllStudents()
    {
        return Ok(_studentService.GetAllStudents());
    }

    // GET: api/students/1
    [HttpGet("{id}")]
    public ActionResult<Student> GetStudentById(int id)
    {
        var student = _studentService.GetStudentById(id);

        if (student == null)
            return NotFound(new { message = "Student not found." });

        return Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public ActionResult<Student> AddStudent(CreateStudentDto studentDto)
    {
        var student = new Student
        {
            Name = studentDto.Name,
            Email = studentDto.Email,
            Age = studentDto.Age
        };

        var createdStudent = _studentService.AddStudent(student);

        return CreatedAtAction(
            nameof(GetStudentById),
            new { id = createdStudent.Id },
            createdStudent);
    }

    // PUT: api/students/1
    [HttpPut("{id}")]
    public ActionResult<Student> UpdateStudent(int id, Student updatedStudent)
    {
        var student = _studentService.UpdateStudent(id, updatedStudent);

        if (student == null)
            return NotFound(new { message = "Student not found." });

        return Ok(student);
    }

    // DELETE: api/students/1
    [HttpDelete("{id}")]
    public IActionResult DeleteStudent(int id)
    {
        var deleted = _studentService.DeleteStudent(id);

        if (!deleted)
            return NotFound(new { message = "Student not found." });

        return NoContent();
    }
}



