using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthyNorka_Terekhov;
public class WorkShift
{
    [Key]
    public int Id { get; set; }
    
    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime? TimeStartShift { get; set; }
    
    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime? TimeEndShift { get; set; }
    public int? AmountWorkHours { get; set; } 
    
    public int EmployeeId { get; set; }
    
    public Employee? Employee { get; set; }
}