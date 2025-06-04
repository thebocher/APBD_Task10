using APBD_Task10.App.DTOs.Account;
using APBD_Task10.App.Helpers;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.AspNetCore.Identity;

namespace APBD_Task10.App.Services;

public class AccountService(MasterContext context) : IAccountService
{
    private readonly MasterContext _context = context;
    private readonly PasswordHasher<Account> _passwordHasher = new ();
    
    public void Register(RegisterRequestDto dto)
    {
        var account = new Account()
        {
            Username = dto.Username,
            EmployeeId = dto.EmployeeId,
            RoleId = dto.RoleId,
        };
        
        account.Password = _passwordHasher.HashPassword(account, dto.Password);
        
        _context.Accounts.Add(account);
        _context.SaveChanges();
    }
}