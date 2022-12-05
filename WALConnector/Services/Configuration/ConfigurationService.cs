using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoLoginConnector.Services.Configuration;

public static class ConfigurationService
{
    private const string ConfigFilename = "logininfo.cfg";

    internal static async Task<LoginConfig?> Load()
    {
        if (!File.Exists(ConfigFilename))
        {
            await File.WriteAllTextAsync(ConfigFilename, JsonSerializer.Serialize(new LoginConfig()));
            return null;
        }
        try
        {
            return JsonSerializer.Deserialize<LoginConfig>(await File.ReadAllTextAsync(ConfigFilename));
        }
        catch
        {
            return null;
        }
    }
}