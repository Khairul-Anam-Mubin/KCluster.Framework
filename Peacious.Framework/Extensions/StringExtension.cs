using System.Text.Json;

namespace Peacious.Framework.Extensions;

public static class StringExtenstion
{
    public static T? Deserialize<T>(this string? str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return default;
        }

        try
        {
            var model = JsonSerializer.Deserialize<T>(str, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return model;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return default;
    }
}