using System;


namespace OsmiumNucleus;


[AttributeUsage(AttributeTargets.All)]
public class EditorOnly : Attribute;

[AttributeUsage(AttributeTargets.All)]
public class UnsafeInternal : Attribute;