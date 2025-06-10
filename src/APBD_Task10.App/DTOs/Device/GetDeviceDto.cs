namespace APBD_Task10.App.DTOs;

public class GetDeviceDto
{
    public required string Name {get; set;}
    public required string Type {get; set;}
    public required bool IsEnabled {get; set;}
    public required object? AdditionalProperties {get; set;}
}