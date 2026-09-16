namespace OsmiumNucleus;

/// <summary> Shows that a piece of code is used only for the Editor, and it should be ignored in the compilation phase </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Delegate | AttributeTargets.Event)]
public class EditorOnly : Attribute;