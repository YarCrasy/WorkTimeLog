using System.IO;
using System.Reflection;
using System.Text.Json;

public static class LocalizationService
{
    private static Dictionary<string, string> _localizedStrings;

    public static void LoadLocalization(string cultureCode)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"WorkTimeLog.Resources.{cultureCode}.strings.json";

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        using StreamReader reader = new(stream);
        var json = reader.ReadToEnd();
        _localizedStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    }

    public static string GetString(string key)
    {
        return _localizedStrings.TryGetValue(key, out var value) ? value : key;
    }
}
