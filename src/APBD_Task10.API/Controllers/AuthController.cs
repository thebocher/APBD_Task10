using System.Text;
using APBD_Task10.App;
using APBD_Task10.App.DTOs.Auth;
using APBD_Task10.App.Services;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.API.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly MasterContext _context;
    private readonly PasswordHasher<Account> _passwordHasher = new PasswordHasher<Account>();
    private readonly ITokenService _tokenService;

    public AuthController(MasterContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    
    [HttpPost]
    public IResult login(LoginDto dto)
    {
        var account = _context.Accounts
            .Include(a => a.Role)
            .FirstOrDefault(a => a.Username == dto.Username);

        if (account == null)
        {
            return Results.BadRequest();
        }
        
        var result = _passwordHasher.VerifyHashedPassword(account, account.Password, dto.Password); 
        if (result == PasswordVerificationResult.Failed)
        {
            return Results.Unauthorized();
        }

        var token = new
        {
            AccessToken = _tokenService.GenerateToken(dto.Username, account.Role.Name),
        };
        
        return Results.Ok(token);

    }
}