namespace APBD_Task10.App.DTOs.Employee;

public class GetEmployeeDto
{
    public required GetEmployeePersonDto Person { get; set; }
    public required decimal Salary { get; set; }
    public required string Position { get; set; }
    public DateTime HireDate { get; set; }
}