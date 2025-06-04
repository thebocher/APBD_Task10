namespace APBD_Task10.App.DTOs.Account;

public class GetAccountDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int RoleId { get; set; }
    public int EmployeeId { get; set; }
    public string Password { get; set; }
}