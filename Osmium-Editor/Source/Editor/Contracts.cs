using System.Reflection;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    public static object? Get<T>(params object[] __args) {
        if (RuntimeModuleCheckInvoker.RuntimeEventReceivers.ContainsKey(typeof(T)))
        {
            return RuntimeModuleCheckInvoker.RuntimeEventReceivers[typeof(T)].GetValue(null);
        }

        Debug.Error("No RuntimeEventReceiver found!");
        return null;
    }
    
    public static void Invoke<T>(params object[] __args) {
        List<MethodInfo> calledMethods = [];
        
        foreach (Type type in RuntimeModuleEventInvoker.GetAllBaseTypes(typeof(T)))
        {
            if (RuntimeModuleEventInvoker.RuntimeEventReceivers.TryGetValue(type, out List<MethodInfo> methods))
            {
                foreach (MethodInfo method in methods)
                {
                    if (calledMethods.Contains(method)) continue;
                    
                    calledMethods.Add(method);
                    
                    try {
                        //method is static so we use null
                        method.Invoke(null, __args);
                    }
                    catch {
                        Debug.Error("Failure invoking method!");
                    }
                }
            }
        }
    }
}