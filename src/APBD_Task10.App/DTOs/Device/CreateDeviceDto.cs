using System.ComponentModel.DataAnnotations;

namespace APBD_Task10.App.DTOs;

public class CreateDeviceDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public required string DeviceTypeName { get; set; }
    
    [Required]
    public required bool IsEnabled { get; set; }
    
    [Required]
    public required string AdditionalProperties { get; set; }
}