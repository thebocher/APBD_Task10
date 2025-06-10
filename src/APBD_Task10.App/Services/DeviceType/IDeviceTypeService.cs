using APBD_Task10.App.DTOs.DeviceType;
using APBD_Task10.Infrastructure;

namespace APBD_Task10.App.Services;

public interface IDeviceTypeService
{
    public List<GetDeviceTypeListItemDto> GetDeviceTypes();
}