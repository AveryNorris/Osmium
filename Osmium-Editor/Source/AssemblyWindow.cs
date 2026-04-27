using System.Reflection;
using OsmiumNucleus;


namespace OsmiumEditor.Source;


//todo: i dont know what to name this or where it should go, it is just an interface for the assembly. I want backend seperate so i think it is important

public static class AssemblyWindow
{
    public static Type[] GetComponents() {
        if (Context.LoadedProgram == null) return [];

        List<Type> returnValue = [];
        
        foreach (Assembly assembly in Context.LoadedProgram.Assemblies) {
            foreach (Type type in assembly.GetTypes()) {
                if (type.IsSubclassOf(typeof(Component))) {
                    returnValue.Add(type);
                }
            }
        }
        
        return returnValue.ToArray();
    }
}