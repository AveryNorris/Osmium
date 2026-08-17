using System.Reflection;
using System.Runtime.Loader;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    public static readonly AssemblyLoadContext _EditorModules = new AssemblyLoadContext(null, false);

    //should be runtime assemblies, since non-module (user source code) is inside it
    public static AssemblyLoadContext _RuntimeModules = new AssemblyLoadContext(null, true);

    public static Assembly[] _ModuleAssemblies => [.._EditorModules.Assemblies, .._RuntimeModules.Assemblies];

    public static Assembly[] _DefaultAssemblies = [typeof(Bedrock).Assembly, typeof(Osmium).Assembly, typeof(Editor).Assembly];

    public static Assembly[] _WorkingAssemblies => [.._ModuleAssemblies, .._DefaultAssemblies];


    public static Type? FindRuntimeModuleType(string __typeName)
    {
        foreach (Assembly assembly in _RuntimeModules.Assemblies)
        {
            Type foundType = assembly.GetType(__typeName);
            
            if(foundType != null) return foundType;
        }

        return null;
    }
    
    public static Type? FindEditorModuleType(string __typeName)
    {
        foreach (Assembly assembly in _EditorModules.Assemblies)
        {
            Type foundType = assembly.GetType(__typeName);
            
            if(foundType != null) return foundType;
        }

        return null;
    }
    
    public static Type? FindModuleType(string __typeName)
    {
        foreach (Assembly assembly in _ModuleAssemblies)
        {
            Type foundType = assembly.GetType(__typeName);
            
            if(foundType != null) return foundType;
        }

        return null;
    }
    
    public static Type? FindType(string __typeName)
    {
        foreach (Assembly assembly in _WorkingAssemblies)
        {
            Type foundType = assembly.GetType(__typeName);
            
            if(foundType != null) return foundType;
        }

        return null;
    }
}