
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.Loader;
using OsmiumNucleus;
using OsmiumRadium;
using Debug = OsmiumNucleus.Debug;

namespace OsmiumEditor;


/// <summary> Manages and points to a project, reload causes it to resolve assemblies but the project remains the same</summary>
public static class Context
{
    
    

    public static AssemblyLoadContext? LoadedProgram;
    
    
    
    //tracking events required! Everything must be able to change at runtime!
    
    //all are reloaded
    public static event EventHandler OnReload;
    
    //all previous occurences of the asembly are unloaded
    public static event EventHandler OnUnload;
    
    //todo: osmium isnt made to exit out of a project or anything so dont worry about that
    
    
    


    public static void Reload()
    {
        MemoryStream assemblyStream = ScriptCompiler.CompileScripts();

        if (!ScriptCompiler.Success)  { Debug.LogError("Source did not compile! "); return; }
            
        Stopwatch timer = Stopwatch.StartNew();
        
        Debug.LogAction("Reloading Osmium!");
        

        //keep old scripts if new ones do not compiles

        OnUnload?.Invoke(null, EventArgs.Empty);



        //unload old program
        LoadedProgram?.Unload();


        LoadedProgram = null;

        GC.Collect();
        GC.WaitForPendingFinalizers();

        LoadedProgram = new AssemblyLoadContext(null, true);

        LoadedProgram.LoadFromStream(assemblyStream);

        assemblyStream.Dispose();

        ModuleManager.AppendModules();

        Debug.LogAction("Finished Compiling and Loading program!");


        Osmium.VirtualInitialize(LoadedProgram.Assemblies);
            
        OnReload?.Invoke(null, EventArgs.Empty);

        timer.Stop();
        Debug.LogAction("Reload finished in: " + timer.Elapsed.Milliseconds + "ms!");

        //todo: wait to reload if a game is running
    }

    //assumes a project was opened from the select menu
    //todo: move all to editor?
    public static void OpenProject(string __path)
    {
        
        Debug.LogAction("Opening project! ", ["Path"], [__path]);
        
        //check for valid project
        string parentDirectory = Path.GetDirectoryName(__path);
        
        Project.ProjectPath = parentDirectory;
        
        UpdateTracker.Initialize();
        
        Reload();
    }
}