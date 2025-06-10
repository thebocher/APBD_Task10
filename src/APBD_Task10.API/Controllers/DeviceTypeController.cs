using APBD_Task10.App.DTOs.DeviceType;
using APBD_Task10.App.Services;
using APBD_Task10.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task10.API.Controllers;


[ApiController]
[Route("/api/device/types")]
public class DeviceTypeController : ControllerBase
{
    private readonly IDeviceTypeService _deviceTypeService;

    public DeviceTypeController(IDeviceTypeService deviceTypeService)
    {
        _deviceTypeService = deviceTypeService;
    }

    [HttpGet]
    public List<GetDeviceTypeListItemDto> All()
    {
        return _deviceTypeService.GetDeviceTypes();
    }
}