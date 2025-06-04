using APBD_Task10.Infrastructure;

namespace APBD_Task10.App.DTOs;

public class GetEmployeeDto
{
    public required Person Person { get; set; }
    public required decimal Salary { get; set; }
    public required PositionDto Position { get; set; }
    public DateTime HireDate { get; set; }
}