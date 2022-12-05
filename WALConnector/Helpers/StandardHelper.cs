using System;
using System.Text.Json;

namespace Autologin.Helpers;

internal static class StandardHelper
{
    public static T DeepClone<T>(this T obj)
        => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj))
        ?? throw new InvalidOperationException("This object couldn't be cloned.");
}