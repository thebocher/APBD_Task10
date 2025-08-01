using APBD_Task10.App.DTOs;
using APBD_Task10.App.DTOs.Device;

namespace APBD_Task10.App.Services;

public interface IDeviceService
{
    public List<GetDeviceListItemDto> GetDevices();
    public GetDeviceDto? GetDevice(int id);
    public void CreateDevice(CreateDeviceDto createDeviceDto);
    public void UpdateDevice(int id, CreateDeviceDto createDeviceDto);
    public void DeleteDevice(int id);
}