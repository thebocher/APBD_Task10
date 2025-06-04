using System.Text.Json;
using System.Text.Json.Serialization;
using APBD_Task10.App.DTOs;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace APBD_Task10.App.Services;

public class DeviceService(MasterContext context) : IDeviceService
{
    private readonly MasterContext _context = context;

    public List<GetDeviceListItemDto> GetDevices()
    {
        return _context.Devices.Select(d => new GetDeviceListItemDto
        {
            Id = d.Id,
            Name = d.Name
        }).ToList();
    }
    
    public GetDeviceDto? GetDevice(int id)
    {
        var device = _context.Devices
            .Include(d => d.DeviceType)
            .Include(d => d.DeviceEmployees)
            .ThenInclude(de => de.Employee)
            .ThenInclude(e => e.Person)
            .Where(d => d.Id == id)
            .Select(d => new
            {
                Name = d.Name,
                DeviceTypeName = d.DeviceType != null ? d.DeviceType.Name : "",
                IsEnabled = d.IsEnabled,
                AdditionalProperties = d.AdditionalProperties,
                CurrentEmployee = d.DeviceEmployees
                    .Where(de => de.ReturnDate == null)
                    .Select(de => new GetDeviceEmployeeDto()
                    {
                        Id = de.Id,
                        Name = de.Employee.Person.GetFullName(),
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefault();

        if (device == null) return null;

        var result = new GetDeviceDto
        {
            Name = device.Name,
            DeviceTypeName = device.DeviceTypeName,
            IsEnabled = device.IsEnabled,
            AdditionalProperties = JsonSerializer.Deserialize<object>(device.AdditionalProperties),
            CurrentEmployee = device.CurrentEmployee
        };
        return result;
    }

    public void CreateDevice(CreateDeviceDto createDeviceDto)
    {
        var deviceType = _context.DeviceTypes
            .FirstOrDefault(dt => dt.Name == createDeviceDto.DeviceTypeName);
        
        if (deviceType == null)
            throw new Exception($"Device type {createDeviceDto.DeviceTypeName} not found");

        var device = new Device
        {
            Name = createDeviceDto.Name,
            DeviceType = deviceType,
            IsEnabled = createDeviceDto.IsEnabled,
            AdditionalProperties = createDeviceDto.AdditionalProperties,
        };
        _context.Devices.Add(device);
        _context.SaveChanges();
    }

    public void UpdateDevice(int id, CreateDeviceDto createDeviceDto)
    {
        var oldDevice = _context.Devices
            .FirstOrDefault(d => d.Id == id);
        if (oldDevice == null)
            throw new Exception($"Device {createDeviceDto.Name} not found");
            
        var deviceType = _context.DeviceTypes
            .FirstOrDefault(dt => dt.Name == createDeviceDto.DeviceTypeName);
        
        if (deviceType == null)
            throw new Exception($"Device type {createDeviceDto.DeviceTypeName} not found");

        oldDevice.Name = createDeviceDto.Name;
        oldDevice.DeviceType = deviceType;
        oldDevice.IsEnabled = createDeviceDto.IsEnabled;
        oldDevice.AdditionalProperties = createDeviceDto.AdditionalProperties;
        _context.Devices.Update(oldDevice);
        _context.SaveChanges();
    }

    public void DeleteDevice(int id)
    {
        var device = new Device
        {
            Id = id
        };
        _context.Devices.Attach(device);
        _context.Devices.Remove(device);
        _context.SaveChanges();
    }
 }