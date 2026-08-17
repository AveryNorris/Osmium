namespace OsmiumNucleus;

/// <summary> Used to mark any method you would like to call once Osmium initializes.
/// Only static, parameterless, methods may be called. Your code will compile regardless, but your event will be ignored at runtime. </summary>
[AttributeUsage(AttributeTargets.Method)]
public class OnInitialize : Attribute;