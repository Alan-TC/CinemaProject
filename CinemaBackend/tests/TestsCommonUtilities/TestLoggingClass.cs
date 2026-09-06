using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Xunit.Abstractions;

namespace TestsCommonUtilities;

public class TestLoggingClass(ITestOutputHelper output)
{
    protected readonly ITestOutputHelper _output = output;

    private readonly JsonSerializerOptions jsonOptions = new() { 
        WriteIndented = true,
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { IncludePrivateFieldsModifier, IgnoreStreamPropertiesModifier }
            }
        };

    public void ExecuteWithContext(object? context, Action testAction)
    {
        try
        {
            testAction();
        }
        catch (Exception)
        {
            LogContext(context);
            throw;
        }
    }

    public async Task ExecuteWithContext(object? context, Func<Task> testAction)
    {
        try
        {
            await testAction();
        }
        catch (Exception)
        {
            LogContext(context);
            throw;
        }
    }

    private void LogContext(object? context)
    {
        if (context is null)
            return;

        _output.WriteLine("Test failed with the following context:");

        try
        {
            _output.WriteLine(JsonSerializer.Serialize(context, jsonOptions));
        }
        catch (Exception ex)
        {
            _output.WriteLine($"[Context serialization failed: {ex.Message}]");
            _output.WriteLine(context.ToString() ?? string.Empty);
        }
    }

    static void IncludePrivateFieldsModifier(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        if (typeInfo.Type.Namespace?.StartsWith("System") == true)
            return;

        var privateFields = typeInfo.Type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => !f.Name.Contains('<'));

        foreach (var field in privateFields)
        {
            var jsonProp = typeInfo.CreateJsonPropertyInfo(field.FieldType, field.Name);
            jsonProp.Get = obj => field.GetValue(obj);
            jsonProp.Set = (obj, value) => field.SetValue(obj, value);

            typeInfo.Properties.Add(jsonProp);
        }
    }

    static void IgnoreStreamPropertiesModifier(JsonTypeInfo typeInfo)
    {
        if (typeof(Stream).IsAssignableFrom(typeInfo.Type))
        {
            for (int i = typeInfo.Properties.Count - 1; i >= 0; i--)
            {
                var propName = typeInfo.Properties[i].Name;
                if (propName is nameof(Stream.ReadTimeout) or nameof(Stream.WriteTimeout))
                {
                    typeInfo.Properties.RemoveAt(i);
                }
            }
        }
    }
}
