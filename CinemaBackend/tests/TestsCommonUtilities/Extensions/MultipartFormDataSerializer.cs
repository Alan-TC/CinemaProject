using System.Collections;
using System.Net.Http.Headers;
using System.Reflection;

namespace TestsCommonUtilities.Extensions;

public static class MultipartFormDataSerializer
{
    public static MultipartFormDataContent Serialize(object obj)
    {
        var formData = new MultipartFormDataContent();
        PopulateFormData(formData, obj, prefix: string.Empty);
        return formData;
    }

    private static void PopulateFormData(MultipartFormDataContent formData, object? obj, string prefix)
    {
        if (obj is null)
            return;

        var type = obj.GetType();

        if (IsSimpleType(type))
        {
            var stringValue = FormatSimpleValue(obj);
            if (stringValue is not null)
            {
                formData.Add(new StringContent(stringValue), prefix);
            }
            return;
        }

        if (obj is Stream stream)
        {
            if (stream.CanSeek)
                stream.Position = 0;

            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            formData.Add(streamContent, prefix, $"{prefix.Split('.').Last().ToLower()}.jpg");
            return;
        }

        if (obj is byte[] bytes)
        {
            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            formData.Add(byteContent, prefix, $"{prefix.Split('.').Last().ToLower()}.jpg");
            return;
        }

        if (obj is IEnumerable enumerable)
        {
            int index = 0;
            foreach (var item in enumerable)
            {
                var indexedKey = $"{prefix}[{index}]";
                PopulateFormData(formData, item, indexedKey);
                index++;
            }
            return;
        }

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in properties)
        {
            var value = prop.GetValue(obj);
            if (value is null)
                continue;

            var key = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
            PopulateFormData(formData, value, key);
        }
    }

    private static bool IsSimpleType(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        return underlyingType.IsPrimitive
            || underlyingType.IsEnum
            || underlyingType == typeof(string)
            || underlyingType == typeof(decimal)
            || underlyingType == typeof(DateTime)
            || underlyingType == typeof(TimeSpan)
            || underlyingType == typeof(Guid);
    }

    private static string? FormatSimpleValue(object value)
    {
        return value switch
        {
            DateTime dt => dt.ToString("o"),
            TimeSpan ts => ts.ToString(),
            Enum e => Convert.ToInt32(e).ToString(),
            _ => value.ToString()
        };
    }
}
