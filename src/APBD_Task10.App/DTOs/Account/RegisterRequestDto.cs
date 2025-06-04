using System.ComponentModel.DataAnnotations;

namespace APBD_Task10.App.DTOs.Account;

public class RegisterRequestDto
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public int EmployeeId { get; set; }
    
    [Required]
    public int RoleId { get; set; }
}