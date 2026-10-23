namespace OsmiumNucleus;

/// <summary>
/// 
/// </summary>
/// <param name="__ignoreVirtualization"></param>
[AttributeUsage(AttributeTargets.Method)]
public class OnInitialize(bool __ignoreVirtualization = false) : Attribute
{
    public readonly bool IgnoreVirtualization = __ignoreVirtualization;
}