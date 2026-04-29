

using System.Reflection;
using OsmiumNucleus;


namespace OsmiumEditor;


public static class ModuleManager
{
    
    public static string[] InternalModules = [
        //Assembly.GetAssembly(typeof(Package)).Location,
        //it seems like since Osmium and Radium are dependencies of Editor it actually breaks? 
        //Assembly.GetAssembly(typeof(Osmium)).Location,
        //Assembly.GetAssembly(typeof(Radium)).Location,
    ];
    
    //metadatareference.creatrfromfile(assembly.location)
    
    public static void AppendModules() {

        foreach (string __modulePath in Directory.GetFiles(Project.ModulesPath, "*.dll", SearchOption.TopDirectoryOnly)) {
            Debug.Action("Found and appending module type! " + __modulePath);
            
            Context.LoadedProgram!.LoadFromAssemblyPath(__modulePath);
            //Radium.LoadedProgram!.LoadFromAssemblyPath(__modulePath);
        }
        
        foreach (string assemblyLocation in InternalModules) {
            Debug.Action("Found and appending internal module! " + assemblyLocation);
            
            Context.LoadedProgram!.LoadFromAssemblyPath(assemblyLocation);
        }
    }
}