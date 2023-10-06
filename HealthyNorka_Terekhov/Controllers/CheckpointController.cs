using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyNorka_Terekhov;

[ApiController]
[Route("[controller]")]
public class CheckpointController : Controller
{
    /// <summary>
    /// Открытие смены
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    ///         id : 1, 
    ///    startShift : "1990/12/11 10:11",
    ///    endShift : "1990/12/11 19:11"
    /// </remarks>
    [HttpPost("StartShift")]
    public IActionResult StartShift([Required] int id, [Required] string startShift, string endShift)
    {
        DateTime dateStart;
        DateTime dateEnd;
        try
        {
            dateStart = DateTime.Parse(startShift);
            dateEnd = DateTime.Parse(endShift);
        }
        catch
        {
            return BadRequest("Даты введены некорректно");
        }

        using (var db = new ApplicationContext())
        {
            var employee = db.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return BadRequest("Сотрудник не найден");
            }

            var shifts = db.WorkShifts
                .Where(x => x.EmployeeId == id)
                .ToList();

            if (shifts.Count > 1)
            {
                var lastShift = shifts[^1];
                if (lastShift.TimeEndShift == null)
                {
                    return BadRequest("Предыдущая смена открыта, необходимо ее закрыть");
                }
            }
            
            var newShift = new WorkShift() { TimeStartShift = dateStart, EmployeeId = id };
            db.WorkShifts.Add(newShift);
            db.SaveChanges();
        }

        return Ok(200);
    }

    /// <summary>
    /// Закрытие смены
    /// </summary>
    /// <remarks> 
    /// Пример запроса:
    ///         id : 1, 
    ///    startShift : "1990/12/11 10:11",
    ///    endShift : "1990/12/11 19:11"
    /// </remarks>
    [HttpPost("EndShift")]
    public IActionResult EndShift([Required] int id, [Required] string endShift, [Required] string startShift)
    {
        WorkShift lastShift;
        DateTime dateStart;
        DateTime dateEnd;
        try
        {
            dateStart = DateTime.Parse(startShift);
            dateEnd = DateTime.Parse(endShift);
        }
        catch
        {
            return BadRequest("Даты введены некорректно");
        }

        using (var db = new ApplicationContext())
        {
            var employee = db.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return BadRequest("Сотрудник не найден");
            }

            var shifts = db.WorkShifts
                .Where(x => x.EmployeeId == id)
                .ToList();

            if (shifts.Count > 1)
            {
                 lastShift = shifts[^1];
                if (lastShift.TimeStartShift == null || lastShift.TimeEndShift != null)
                {
                    return BadRequest("Вы не открыли смену или уже закрыли смену за этот день");
                }
            }

            if (dateEnd < dateStart)
            {
                return BadRequest("Время закрытия смены не может быть меньше времени открытия");
            }

            lastShift.TimeEndShift = dateEnd;
            lastShift.AmountWorkHours = dateEnd.Hour - dateStart.Hour;
            db.Update(lastShift);
            db.SaveChanges();
        }

        return Ok(200);
    }
}
