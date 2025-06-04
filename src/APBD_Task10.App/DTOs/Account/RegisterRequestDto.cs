using System.ComponentModel.DataAnnotations;

namespace APBD_Task10.App.DTOs.Account;

public class RegisterRequestDto
{
    [Required]
    [RegularExpression("^[^0-9].+")]
    public string Username { get; set; }
    
    [Required]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{12,}$")]
    public string Password { get; set; }
    
    [Required]
    public int EmployeeId { get; set; }
    
    [Required]
    public int RoleId { get; set; }
}