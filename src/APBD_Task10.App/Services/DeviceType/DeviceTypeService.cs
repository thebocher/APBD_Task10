using APBD_Task10.App.DTOs.DeviceType;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;

namespace APBD_Task10.App.Services;

public class DeviceTypeService : IDeviceTypeService
{
    private readonly MasterContext _context;

    public DeviceTypeService(MasterContext context)
    {
        _context = context;
    }
    
    public List<GetDeviceTypeListItemDto> GetDeviceTypes()
    {
        return _context.DeviceType
            .Select(d => new GetDeviceTypeListItemDto
            {
                Id = d.Id,
                Name = d.Name,
            })
            .ToList();
    }
}