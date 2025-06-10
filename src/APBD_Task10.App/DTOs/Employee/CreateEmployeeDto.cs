namespace APBD_Task10.App.DTOs.Employee;

public class CreateEmployeeDto
{
    public required CreateEmployeePersonDto Person { get; set; }
    public required decimal Salary { get; set; }
    public required int PositionId { get; set; }
}