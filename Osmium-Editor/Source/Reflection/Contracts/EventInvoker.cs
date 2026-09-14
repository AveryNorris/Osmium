using System.Reflection;
using OsmiumNucleus;

namespace OsmiumEditor;

public static class RuntimeModuleEventInvoker
{
    
    public static Dictionary<Type, List<MethodInfo>> RuntimeEventReceivers = new Dictionary<Type, List<MethodInfo>>();

    public static void OnLoad() {
        foreach (Assembly assembly in Editor._ModuleAssemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.IsStatic)
                    {
                        IEnumerable<Attribute> attributes = method.GetCustomAttributes();

                        foreach (Attribute attribute in attributes)
                        {
                            if (attribute.GetType().IsAssignableTo(typeof(RuntimeModuleEvent)))
                            {
                                //counts all inherited types, so if an event inherits another event it counts all levels
                                IEnumerable<Type> AllTypes = GetAllBaseTypes(attribute.GetType());

                                foreach (Type t in AllTypes)
                                {
                                    if (RuntimeEventReceivers.ContainsKey(t))
                                    {
                                        RuntimeEventReceivers[t].Add(method);
                                    }
                                    else
                                    {
                                        RuntimeEventReceivers.Add(t, [method]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    internal static IEnumerable<Type> GetAllBaseTypes(Type type) {
        List<Type> AllTypes = [type];

        Type? currentType = type;
        while (currentType.IsAssignableTo(typeof(RuntimeModuleEvent)))
        {
            currentType = currentType.BaseType;

            if (currentType == null || currentType == typeof(RuntimeModuleEvent)) break;
                                    
            AllTypes.Add(currentType);
        }

        return AllTypes;
    }

    public static void OnUnload() {
        RuntimeEventReceivers.Clear();
    }
}