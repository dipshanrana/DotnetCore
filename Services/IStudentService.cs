using ASPNETWEBAPI.Models;

namespace ASPNETWEBAPI.Services;

public interface IStudentService
{
    List<Student> GetAll();
    Student? GetById(string id);
    void Add(Student student);
    bool Update(Student student);
    bool Delete(string id);
}
