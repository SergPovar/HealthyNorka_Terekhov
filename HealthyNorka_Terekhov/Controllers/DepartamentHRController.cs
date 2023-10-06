using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HealthyNorka_Terekhov;

[ApiController]
[Route("[controller]")]
public class DepartamentHRController : Controller
{
    /// <summary>
    /// Добавление сотрудника
    /// </summary>
    /// <remarks> 
    /// Пример запроса:
    ///      "lastName" : "Иванов",
    ///      "firstName":"Иван",
    ///      "jobTitle" : "Engineer"
    /// </remarks>
    [HttpPost("AddEmployee")]
    public IActionResult AddEmployee([Required] string lastName, [Required] string firstName, [Required] JobTitles jobTitle)
    {
        if (jobTitle == null || lastName == null || firstName == null)
        {
            return NotFound("Не заполнены необходимые поля");;
        }

        var employee = new Employee() { FirstName = firstName, LastName = lastName, JobTitle = jobTitle };
        using (var db = new ApplicationContext())
        {
            db.Add(employee);
            db.SaveChanges();
        }

        return Ok(employee);
    }

    /// <summary>
    /// Обновить данные сотрудника
    /// </summary>
    /// <remarks> 
    /// Пример запроса:
    ///     "Id" : "1"
    ///     "lastName" : "Иванов",
    ///     "firstName":"Иван",
    ///     "jobTitle" : "Engineer"
    /// </remarks>
    [HttpPost("UpdateEmployee")]
    public IActionResult UpdateEmployee([Required] int id, string? lastName, string? firstName, JobTitles jobTitles)
    {
       Employee employee;
        using (var db = new ApplicationContext())
        {
           employee = db.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound("Сотрудника с таким Id не существует");
            }

            if (lastName != null)
            {
                employee.LastName = lastName;
            }

            if (firstName != null)
            {
                employee.FirstName = firstName;
            }

            if (jobTitles != null)
            {
                employee.JobTitle = jobTitles;
            }

            db.Employees.Update(employee);
            db.SaveChanges();
        }
        return Ok(employee);
    }

    /// <summary>
    /// Получить список сотрудников
    /// </summary>
    /// <remarks> 
    /// Пример запроса:
    ///      "jobTitle" : "Engineer"
    /// </remarks>
    [HttpGet("GetEmployees")]
    public IActionResult GetEmployees(JobTitles jobTitle)
    {
        var employee = new List<Employee>();

        using (var db = new ApplicationContext())
        {
            if (jobTitle == JobTitles.НетДолжности)
            {
                employee = db.Employees.ToList();
            }
            else
            {
                employee = db.Employees.ToArray()
                    .Where(x => x.JobTitle == jobTitle)
                    .ToList();
            }
        }

        if (employee.Count<1)
        {
            return NotFound("Список сотрудников пуст");
        }
        return Ok(employee);
    }

    /// <summary>
    /// Удалить сотрудника
    /// </summary>
    /// <remarks> 
    /// Пример запроса:
    ///      "Id" : "1"
    /// </remarks>
    [HttpDelete("DeleteEmployee")]
    public IActionResult DeleteEmployee([Required] int id)
    {
        using (var db = new ApplicationContext())
        {
            var employee = db.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound("Сотрудника с таким Id не существует");
            }

            db.Employees.Remove(employee);
            db.SaveChanges();
        }
        return Ok(200);
    }

    /// <summary>
    /// Получить список всех должностей
    /// </summary>
    [HttpGet("GetAllJobTitle")]
    public List<string> GetAllJobTitle()
    {
       var list= Enum.GetNames(typeof(JobTitles)).ToList();
        list.Remove("НетДолжности");
       return list;
    }
}
