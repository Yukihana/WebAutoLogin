using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace WALConnector.Services.Configuration;

public static class ConfigurationService
{
    private const string ConfigFilename = "logininfo.cfg";

    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
    };

    internal static async Task<LoginConfig?> Load()
    {
        if (!File.Exists(ConfigFilename))
        {
            await File.WriteAllTextAsync(ConfigFilename, JsonSerializer.Serialize(new LoginConfig(), _options));
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