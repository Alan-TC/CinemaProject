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
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { IncludePrivateFieldsModifier }
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
        _output.WriteLine(JsonSerializer.Serialize(context, jsonOptions));
    }

    static void IncludePrivateFieldsModifier(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        var privateFields = typeInfo.Type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in privateFields)
        {
            var jsonProp = typeInfo.CreateJsonPropertyInfo(field.FieldType, field.Name);
            jsonProp.Get = obj => field.GetValue(obj);
            jsonProp.Set = (obj, value) => field.SetValue(obj, value);
            
            typeInfo.Properties.Add(jsonProp);
        }
    }
}
