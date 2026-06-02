namespace OsmiumNucleus;

/// <summary> Allows a Component to be updated even if Osmium is only initialized </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AlwaysUpdate : Attribute;