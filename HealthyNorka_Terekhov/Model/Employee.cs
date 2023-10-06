using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthyNorka_Terekhov;

[Serializable]
public class Employee
{
    [Key]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? FatherName { get; set; }
    public JobTitles JobTitle{ get; set; }
    
    public List<WorkShift> WorkShifts { get; set; }
}