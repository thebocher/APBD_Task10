using APBD_Task10.App.DTOs.Account;
using APBD_Task10.App.Helpers;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;

namespace APBD_Task10.App.Services;

public class AccountService(MasterContext context) : IAccountService
{
    private readonly MasterContext _context = context;
    
    public void Register(RegisterRequestDto dto)
    {
        string password = SecurityHelper.GetHashedPassword(dto.Password);

        var account = new Account()
        {
            Username = dto.Username,
            Password = password,
            EmployeeId = dto.EmployeeId,
            RoleId = dto.RoleId,
        };
        
        
    }
}