using System.ComponentModel.DataAnnotations;

namespace APBD_Task10.App.DTOs.Device;

public class CreateDeviceDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required int TypeId { get; set; }
    
    [Required]
    public required bool IsEnabled { get; set; }
    
    [Required]
    public required object AdditionalProperties { get; set; }
}