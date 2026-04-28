public class MemberDatum
{
    public string FieldName { get; set; }
    public object? Value { get; set; }
}

public class JsonIntermediate
{
    public int Depth { get; set; }
    public string AssemblyName { get; set; }
    public string ComponentTypeName { get; set; }
    public string SceneName { get; set; }
    public string ComponentName { get; set; }
    public bool Enabled { get; set; }
    public int Priority { get; set; }
    public List<string> Tags { get; set; }
    public List<MemberDatum> MemberData { get; set; } = [];

    public List<JsonIntermediate> Children { get; set; } = [];
}