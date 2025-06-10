using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using APBD_Task10.API.Helpers.Services;
using APBD_Task10.App.DTOs.Device;
using APBD_Task10.App.Services;
using Microsoft.AspNetCore.Authentication;

namespace APBD_Task10.API.Helpers.Middleware;

public class AdditionalPropertiesValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JsonElement _config;
    private readonly ILogger<AdditionalPropertiesValidationMiddleware> _logger;
    
    public AdditionalPropertiesValidationMiddleware(
        RequestDelegate next, 
        ValidationConfigService validationConfigService,
        ILogger<AdditionalPropertiesValidationMiddleware> logger
        )
    {
        _next = next;
        _config = validationConfigService.GetConfiguration();
        _logger = logger;
    }

    private Dictionary<string, int> GetDeviceTypeIds(IDeviceTypeService deviceTypeService)
    {
        return deviceTypeService.GetDeviceTypes()
            .ToDictionary(x => x.Name, x => x.Id);
    }

    public async Task Invoke(HttpContext context, IDeviceTypeService deviceTypeService)
    {
        context.Request.EnableBuffering();

        // reading json body
        using var reader = new StreamReader(
            context.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        
        context.Request.Body.Position = 0;

        if (!context.Request.Path.StartsWithSegments("/api/devices") || context.Request.Method != "POST" && context.Request.Method != "PUT")
        {
            await _next(context);
            return;
        }

        // deserializing
        var dto = JsonSerializer.Deserialize<CreateDeviceDto>(body, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        
        if (dto == null)
        {
            _logger.LogWarning("Failed to parse the request body to CreateDeviceDto");
            context.Response.StatusCode = 400;
            return;
        }
        
        _logger.LogInformation("Started validating device data");

        var deviceTypeIds = GetDeviceTypeIds(deviceTypeService);

        foreach (var deviceValidation in _config.GetProperty("validations").EnumerateArray())
        {
            var deviceValidationTypeName = deviceValidation.GetString("type")!;
            var deviceValidationTypeId = deviceTypeIds[deviceValidationTypeName];
            if (deviceValidationTypeId != dto.TypeId) continue;
            
            // getting preRequestName
            var preRequestName = deviceValidation.GetString("preRequestName");

            if (preRequestName == null)
            {
                context.Response.StatusCode = 500;
                _logger.LogError($"Device validation failure: preRequestName is null");
                
                return;
            }

            var propertyFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            
            // finding validation rules for device type
            var dtoPreRequestField = dto.GetType().GetProperty(preRequestName, propertyFlags);
            
            if (dtoPreRequestField == null)
            {
                _logger.LogError($"Device validation failure: dto doesnt have property {preRequestName}");
                context.Response.StatusCode = 500;
                return;
            }
            
            // getting preRequestValue
            var preRequestValue = deviceValidation.GetString("preRequestValue");

            if (dtoPreRequestField.GetValue(dto)!.ToString()!.ToLower() != preRequestValue)
            {
                await _next(context);
                _logger.LogDebug($"skipping checking as preRequest value != dto field value");
                return;
            }
            
            var additionalProperties = (JsonElement)dto.AdditionalProperties;

            foreach (var rule in deviceValidation.GetProperty("rules").EnumerateArray())
            {
                string paramName = rule.GetString("paramName")!;
                var paramValue = additionalProperties.GetProperty(paramName).GetString();

                if (paramValue == null)
                {
                    context.Response.StatusCode = 400;
                    _logger.LogInformation($"User didnt provide param {paramName}");
                    return;
                }
                
                var regexProperty = rule.GetProperty("regex");

                switch (regexProperty.ValueKind)
                {
                    case JsonValueKind.Array:
                    {
                        var regexList = regexProperty.EnumerateArray()
                            .Select(e => e.GetString()).ToList();
                        foreach (var regex in regexList)
                        {
                            if (Regex.IsMatch(paramValue, regex!))
                            {
                                await _next(context);
                                return;
                            }
                        }
                        _logger.LogInformation(
                            $"Device validation faliure: {paramValue} doesnt match regex: {string.Join(',', regexList)}"
                            );
                        context.Response.StatusCode = 400;
                        return;
                    }
                    case JsonValueKind.String:
                    {
                        string regex = regexProperty.GetString()!;
                        
                        if (!Regex.IsMatch(paramValue, regex))
                        {
                            _logger.LogInformation($"Device validation faliure: {paramValue} doesnt match regex: {regex}");
                            context.Response.StatusCode = 400;
                            return;
                        }
                        break;
                    }
                        
                }                
                
            }
        }
        
        _logger.LogInformation("device validation passed");

        await _next(context);
    }

}