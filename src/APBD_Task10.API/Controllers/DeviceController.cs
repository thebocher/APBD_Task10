using System.IdentityModel.Tokens.Jwt;
using APBD_Task10.App.DTOs;
using APBD_Task10.App.Services;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Task10.API.Controllers;

[ApiController]
[Route("/api/devices")]
public class DeviceController(IDeviceService deviceService, MasterContext context) : ControllerBase
{
    private readonly IDeviceService _deviceService = deviceService;
    private readonly MasterContext _context = context;
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public List<GetDeviceListItemDto> GetDevices()
    {
        return _deviceService.GetDevices();
    }

    [HttpGet("{id}")]
    [Authorize]
    public IResult GetDevice(int id)
    {
        if (!User.IsInRole("Admin"))
        {
            var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
            
            if (usernameClaim == null) return Results.Forbid();

            var username = usernameClaim.Value;
            
            var accountHasDeviceAssigned = _context.Accounts
                .Include(a => a.Employee)
                .ThenInclude(e => e.DeviceEmployees)
                .ThenInclude(d => d.Device)
                .FirstOrDefault(a => 
                    a.Username == username
                    && a.Employee.DeviceEmployees.Any(e => e.DeviceId == id));
            
            if (accountHasDeviceAssigned == null) 
                return Results.Forbid();
        }
        
        var result = _deviceService.GetDevice(id);

        return result == null ? Results.NotFound() : Results.Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IResult CreateDevice([FromBody] CreateDeviceDto createDeviceDto)
    {
        try
        {
            _deviceService.CreateDevice(createDeviceDto);
            return Results.Created();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
    
    [HttpPut("{id}")]
    [Authorize]
    public IResult UpdateDevice(int id, [FromBody] CreateDeviceDto createDeviceDto)
    {
        if (!User.IsInRole("Admin"))
        {
            var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
            
            if (usernameClaim == null) return Results.Forbid();

            var username = usernameClaim.Value;
            
            var accountHasDeviceAssigned = _context.Accounts
                .Include(a => a.Employee)
                .ThenInclude(e => e.DeviceEmployees)
                .ThenInclude(d => d.Device)
                .FirstOrDefault(a => 
                    a.Username == username
                    && a.Employee.DeviceEmployees.Any(e => e.DeviceId == id));
            
            if (accountHasDeviceAssigned == null) 
                return Results.Forbid();
        }
        
        try
        {
            _deviceService.UpdateDevice(id, createDeviceDto);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult DeleteDevice(int id)
    {
        try
        {
            _deviceService.DeleteDevice(id);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
}