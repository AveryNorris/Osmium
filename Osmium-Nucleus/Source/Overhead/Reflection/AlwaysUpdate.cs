namespace OsmiumNucleus;

/// <summary> Ignores virtualization when receiving events </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AlwaysUpdate : Attribute;