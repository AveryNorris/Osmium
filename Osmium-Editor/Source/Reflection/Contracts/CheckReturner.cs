using System.Reflection;
using OsmiumNucleus;

namespace OsmiumEditor;

public static class RuntimeModuleCheckInvoker
{
    
    public static Dictionary<Type, FieldInfo> RuntimeEventReceivers = new Dictionary<Type, FieldInfo>();

    public static void OnLoad() {
        foreach (Assembly assembly in Editor._ModuleAssemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                foreach (FieldInfo field in type.GetFields())
                {
                    if (field.IsStatic)
                    {
                        IEnumerable<Attribute> attributes = field.GetCustomAttributes();

                        foreach (Attribute attribute in attributes)
                        {
                            if (attribute.GetType().IsAssignableTo(typeof(RuntimeModuleEvent)))
                            {
                                if (RuntimeEventReceivers.ContainsKey(attribute.GetType()))
                                {
                                    Debug.Error("Two modules contain the same Check source! One of them will be ignored!");
                                    break;
                                }
                                    
                                RuntimeEventReceivers.Add(attribute.GetType(), field);
                            }
                        }
                    }
                }
            }
        }
    }

    public static void OnUnload() {
        RuntimeEventReceivers.Clear();
    }
    
}