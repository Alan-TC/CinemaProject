using System.Text.Json.Serialization;

namespace TestsCommonUtilities;

public class TestContext
{
    public Dictionary<string, object?> Context {get;} = [];
    public Dictionary<string, int> _keyCount = [];

    public void Add(object contextObject)
    {
        var typeName = contextObject?.GetType().Name ?? "null";

        _keyCount.TryAdd(typeName, 0);
        _keyCount[typeName]++;
        
        var key = $"{typeName}__{_keyCount[typeName]}";
        Context[key] = contextObject;
    }
}
