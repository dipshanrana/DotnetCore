using Microsoft.AspNetCore.Mvc;
using ASPNETWEBAPI.Models;
using ASPNETWEBAPI.Services;

namespace ASPNETWEBAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // A) Get All Students
    // Endpoint: GET api/student/getall
    [HttpGet("getall")]
    public ActionResult<List<Student>> GetAll()
    {
        return Ok(_studentService.GetAll());
    }

    // B) Get Student by ID
    // Endpoint: GET api/student/{id}
    [HttpGet("{id}")]
    public ActionResult<Student> GetById(string id)
    {
        var student = _studentService.GetById(id);
        if (student == null)
        {
            return NotFound(new { message = $"Student with ID {id} not found." });
        }
        return Ok(student);
    }

    // C) Add a New Student
    // Endpoint: POST api/student/add
    [HttpPost("add")]
    public ActionResult Add(Student student)
    {
        if (student == null || string.IsNullOrEmpty(student.Id))
        {
            return BadRequest(new { message = "Invalid student data." });
        }

        var existing = _studentService.GetById(student.Id);
        if (existing != null)
        {
            return Conflict(new { message = "Student with this ID already exists." });
        }

        _studentService.Add(student);
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    // D) Update an Existing Student
    // Endpoint: PUT api/student/update
    [HttpPut("update")]
    public ActionResult Update(Student student)
    {
        if (student == null || string.IsNullOrEmpty(student.Id))
        {
            return BadRequest(new { message = "Invalid student data." });
        }

        var updated = _studentService.Update(student);
        if (!updated)
        {
            return NotFound(new { message = $"Student with ID {student.Id} not found." });
        }

        return Ok(new { message = "Student updated successfully.", student });
    }

    // E) Delete a Student
    // Endpoint: DELETE api/student/delete/{id}
    [HttpDelete("delete/{id}")]
    public ActionResult Delete(string id)
    {
        var deleted = _studentService.Delete(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Student with ID {id} not found." });
        }

        return Ok(new { message = "Student deleted successfully." });
    }
}
