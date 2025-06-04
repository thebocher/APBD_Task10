using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Task10.Infrastructure;

public class Account
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public int EmployeeId { get; set; }
    
    [ForeignKey("EmployeeId")]
    public virtual Employee Employee { get; set; }
    
    public int RoleId { get; set; }
    
    [ForeignKey("RoleId")]
    public Role Role { get; set; }
}