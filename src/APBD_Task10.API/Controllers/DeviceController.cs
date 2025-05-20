using APBD_Task10.App.DTOs;
using APBD_Task10.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task10.API.Controllers;

[ApiController]
[Route("/api/devices")]
public class DeviceController(IDeviceService deviceService)
{
    private readonly IDeviceService _deviceService = deviceService;
    
    [HttpGet]
    public List<GetDeviceListItemDto> GetDevices()
    {
        return _deviceService.GetDevices();
    }

    [HttpGet("{id}")]
    public IResult GetDevice(int id)
    {
        var result = _deviceService.GetDevice(id);

        return result == null ? Results.NotFound() : Results.Ok(result);
    }

    [HttpPost]
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
    
    [HttpPut]
    public IResult UpdateDevice([FromBody] CreateDeviceDto createDeviceDto)
    {
        try
        {
            _deviceService.UpdateDevice(createDeviceDto);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
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