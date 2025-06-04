using System.IdentityModel.Tokens.Jwt;
using APBD_Task10.App.DTOs;
using APBD_Task10.App.Services;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.API.Controllers;

[ApiController]
[Route("/api/employees")]
public class EmployeeController(IEmployeeService employeeService, MasterContext context) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly MasterContext _context = context;
    
    [HttpGet]
    [Authorize]
    public List<GetEmployeeListItemDto> GetEmployees()
    {
        return _employeeService.GetEmployees();
    }

    [HttpGet("{id}")]
    [Authorize]
    public IResult GetEmployee(int id)
    {
        if (!User.IsInRole("Admin"))
        {
            var usernameClaim = User.Claims.FirstOrDefault(
                c => c.Type == JwtRegisteredClaimNames.Name);
            
            if (usernameClaim == null) 
                return Results.Forbid();

            var username = usernameClaim.Value;

            var employeeAccount = _context.Accounts
                .FirstOrDefault(
                    a => a.Username == username
                        && a.EmployeeId == id   
                    );
            
            if (employeeAccount == null) 
                return Results.Forbid();
        }
        var result = _employeeService.GetEmployee(id);
        
        return result == null ? Results.NotFound() : Results.Ok(result);
    }
}