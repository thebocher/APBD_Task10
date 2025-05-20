namespace APBD_Task10.App.DTOs;

public class CreateDeviceDto
{
    public string Name { get; set; }
    public string DeviceTypeName { get; set; }
    public bool IsEnabled { get; set; }
    public string AdditionalProperties { get; set; }
}