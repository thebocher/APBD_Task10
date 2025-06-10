using System.Text.Json;

namespace APBD_Task10.API.Helpers.Services;

public class ValidationConfigService
{
    private readonly string _validationJsonConfigPath;

    public ValidationConfigService(string validationJsonConfigPath)
    {
            _validationJsonConfigPath = validationJsonConfigPath;
    }

    public JsonElement GetConfiguration()
    {
        return JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(_validationJsonConfigPath));
    }
}