namespace APBD_Task10.App.DTOs.Employee;

public class CreateEmployeePersonDto
{
    public required  string PassportNumber { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
}