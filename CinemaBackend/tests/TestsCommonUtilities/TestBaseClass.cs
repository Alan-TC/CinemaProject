using System.Text.Json;
using Xunit.Abstractions;

namespace TestsCommonUtilities;

public abstract class TestBaseClass(ITestOutputHelper output)
{
    protected readonly ITestOutputHelper _output = output;

    private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };

    protected void ExecuteWithContext(object? context, Action testAction)
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

    protected async Task ExecuteWithContext(object? context, Func<Task> testAction)
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
}
