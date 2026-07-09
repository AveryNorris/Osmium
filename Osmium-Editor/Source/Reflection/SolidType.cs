using System.Reflection;
using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor.Serialization;

public record SolidType
{
    
    
    public readonly bool IsValid = true;
    public readonly string FullName;
    
    public SolidType(object __object) : this(__object?.GetType()) {}
    
    public SolidType(Type __type) {
        if (__type == null) {
            Debug.Error("Cannot make a Solid Type from a null Type!");
            IsValid = false;
            return;
        }

        if (__type.FullName == null) {
            Debug.Error("This type does not have a full name! Please use a normal Type defined from a class.");
            IsValid = false;
            return;
        }
        
        FullName = __type.FullName;
    }

    public static SolidType? FromTypeName(string __typeName) {
        foreach (Assembly assembly in Context.GetAssemblies()) {
            foreach (Type type in assembly.GetTypes()) {
                return new SolidType(type);
            }
        }

        return null;
    }

    public Type? FindType() {
        foreach (Assembly assembly in Context.GetAssemblies()) {
            foreach (Type type in assembly.GetTypes()) {
                if (type.FullName == FullName) {
                    return type;
                }
            }
        }
        
        Debug.Error("A referenced Solid Type no longer exists!");
        
        return null;
    }
    
    
}