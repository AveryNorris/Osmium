
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.Loader;
using OsmiumEditor.ComponentMap;
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
    public static event Action OnReload;
    
    //all previous occurences of the asembly are unloaded
    public static event Action OnUnload;

    private static bool ReloadScheduled;

    public static string ComponentMapPath;
    
    //todo: osmium isnt made to exit out of a project or anything so dont worry about that

    public static void QueueReload() {
        ReloadScheduled = true;
    }

    public static void AbortReload() {
        ReloadScheduled = false;
    }

    public static void Update() {
        if(ReloadScheduled) Reload();
        
        ReloadScheduled = false;
    }


    //todo: ENFORCE/DOCUMENT that actiosn that require another reload should not be tied to reload without manually calling it!
    public static void Reload() {
        Debug.Clear();
        //todo: make method
        UpdateTracker.SurpressReload = true;
        MemoryStream assemblyStream = ScriptCompiler.CompileScripts();

        if (!ScriptCompiler.Success)  { Debug.Error("Source did not compile! "); return; }
        
        OnUnload?.Invoke();


        List<string> Scenes = [];

        foreach (Scene scene in Osmium.Scenes) {

            Scenes.Add(scene.Name);

            //destroy scene change this! and make it attach to scene
            Osmium.RemoveScene(scene);
            scene.DestroyAll();
        }

        Stopwatch timer = Stopwatch.StartNew();
        
        Debug.Action("Reloading Osmium!");
        

        //keep old scripts if new ones do not compiles




        //unload old program
        LoadedProgram?.Unload();


        LoadedProgram = null;

        GC.Collect();
        GC.WaitForPendingFinalizers();

        LoadedProgram = new AssemblyLoadContext(null, true);

        LoadedProgram.LoadFromStream(assemblyStream);

        assemblyStream.Dispose();

        ModuleManager.AppendModules();

        Debug.Action("Finished Compiling and Loading program!");


        List<Assembly> usedAssemblies = LoadedProgram.Assemblies.ToList();
        usedAssemblies.Add(typeof(Package).Assembly);
        
        Osmium.VirtualInitialize(usedAssemblies);
        

        foreach (string sceneName in Scenes) {
            Osmium.AddScene(sceneName);
        }
        
        OnReload?.Invoke();

        UpdateTracker.SurpressReload = false;
        
        timer.Stop();
        Debug.Action("Reload finished in: " + timer.Elapsed.Milliseconds + "ms!");

        //todo: wait to reload if a game is running
    }

    //assumes a project was opened from the select menu
    //todo: move all to editor?
    public static void OpenProject(string __path)
    {
        
        //todo: event
        
        Debug.Action("Opening project! ", ["Path"], [__path]);
        
        //check for valid project
        string parentDirectory = Path.GetDirectoryName(__path);
        
        Project.ProjectPath = parentDirectory;
        
        UpdateTracker.Initialize();
        
        ComponentMapPath = Project.GetProjectSubPath(Path.Combine("Editor", "Component.osmap"), regenerate: true);
        UpdateTracker.BlacklistedPaths.Add(ComponentMapPath);
        
        Reload();
        
        //post reload so the assemblies exist
        
        //todo: check file permissions and throw in the editor if we dont have
    }
}