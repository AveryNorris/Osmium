

using OsmiumNucleus;


namespace OsmiumEditor;


public static class ModuleManager
{
    public static void AppendModules() {

        foreach (string __modulePath in Directory.GetFiles(Project.ModulesPath, "*.dll", SearchOption.TopDirectoryOnly)) {
            Debug.Action("Found and appending module type! " + __modulePath);
            
            Context.LoadedProgram.LoadFromAssemblyPath(__modulePath);
            //Radium.LoadedProgram!.LoadFromAssemblyPath(__modulePath);
        }
    }
}