using ASPNETWEBAPI.Models;

namespace ASPNETWEBAPI.Services;

public class StudentService : IStudentService
{
    private static readonly List<Student> students = new List<Student>
    {
        new Student { Id = "NP01MS7A240036", Name = "John Doe", Email = "john@example.com", Age = 20 },
        new Student { Id = "NP01MS7A240037", Name = "Jane Smith", Email = "jane@example.com", Age = 22 }
    };

    public List<Student> GetAll() => students;

    public Student? GetById(string id) => students.FirstOrDefault(s => s.Id == id);

    public void Add(Student student)
    {
        students.Add(student);
    }

    public bool Update(Student student)
    {
        var index = students.FindIndex(s => s.Id == student.Id);
        if (index == -1) return false;
        
        students[index] = student;
        return true;
    }

    public bool Delete(string id)
    {
        var student = GetById(id);
        if (student == null) return false;
        
        students.Remove(student);
        return true;
    }
}
