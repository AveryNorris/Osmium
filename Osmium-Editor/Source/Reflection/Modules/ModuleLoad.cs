namespace OsmiumEditor;

[AttributeUsage(AttributeTargets.Method)]
public class ModuleLoad : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public class ModuleUnload : Attribute;