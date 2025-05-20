using APBD_Task10.App.DTOs;
using APBD_Task10.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task10.API.Controllers;

[ApiController]
[Route("/api/employees")]
public class EmployeeController(IEmployeeService employeeService)
{
    private readonly IEmployeeService _employeeService = employeeService;
    
    [HttpGet]
    public List<GetEmployeeListItemDto> GetEmployees()
    {
        return _employeeService.GetEmployees();
    }

    [HttpGet("{id}")]
    public IResult GetEmployee(int id)
    {
        var result = _employeeService.GetEmployee(id);
        
        return result == null ? Results.NotFound() : Results.Ok(result);
    }
}