namespace APBD_Task10.App.DTOs;

public class GetDeviceDto
{
    public required string Name {get; set;}
    public required string DeviceTypeName {get; set;}
    public required bool IsEnabled {get; set;}
    public required object? AdditionalProperties {get; set;}
    public required GetDeviceEmployeeDto? CurrentEmployee {get; set;}
}