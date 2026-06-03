using System.Diagnostics;

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

    //yuck
    public const string CsProj = "<Project Sdk=\"Microsoft.NET.Sdk\">\n\n    <PropertyGroup>\n        <TargetFramework>net10.0</TargetFramework>\n        <ImplicitUsings>enable</ImplicitUsings>\n        <Nullable>disable</Nullable>\n    </PropertyGroup>\n\n    <ItemGroup>\n      <Reference Include=\"Osmium-Nucleus\">\n        <HintPath>/Users/averynorris/Osmium/Osmium-Nucleus/bin/Debug/net10.0/Osmium-Nucleus.dll</HintPath>\n      </Reference>\n    </ItemGroup>";

    public static Assembly[] GetAssemblies() {
        List<Assembly> assemblies = [];
        assemblies.AddRange(LoadedProgram.Assemblies);
        assemblies.AddRange(typeof(Context).Assembly, typeof(Component).Assembly, typeof(Backend).Assembly);
        
        return assemblies.ToArray();
    }
    
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
        
        if(Osmium.IsVirtualized)
            Osmium.VirtualClose();
        
        //todo: make method
        UpdateTracker.SurpressReload = true;
        MemoryStream assemblyStream = ScriptCompiler.CompileScripts();

        if (!ScriptCompiler.Success)  { Debug.Error("Source did not compile! "); return; }
        
        OnUnload?.Invoke();
        
        string csProjAppenditures = "\n\n";

        foreach (string file in Directory.GetFiles(Project.RuntimeModulesPath, "*.dll", SearchOption.AllDirectories))
        {
            csProjAppenditures += "<ItemGroup>\n        <Reference Include=\"" + Path.GetFileNameWithoutExtension(file) + "\"><HintPath>" + file + "</HintPath></Reference>\n    </ItemGroup>";
        }
        
        File.WriteAllText(Path.Combine(Project.ProjectPath, "project.csproj"), CsProj + csProjAppenditures + "</Project>\n");


        List<string> Scenes = [];

        foreach (Scene scene in Osmium.Scenes) {

            Scenes.Add(scene.Name);

            //destroy scene change this! and make it attach to scene
            Osmium.RemoveScene(scene);
            scene.DestroyAll();
        }

        Stopwatch timer = Stopwatch.StartNew();
        
        Debug.Action("Reloading Osmium!");
        
        Backend.Clear();

        Backend.Add<EditorOverhead>();
        Backend.Add<DebugOverlay>();
        

        //keep old scripts if new ones do not compiles

    //todo: clear old reflection types in event manager in osmium? 
    
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
        
        Debug.Log("Assemblies! " + string.Join(", ", usedAssemblies.ToArray()));
        
        Osmium.VirtualInitialize(usedAssemblies);
        

        foreach (string sceneName in Scenes) {
            Osmium.AddScene(sceneName);
        }
        
        OnReload?.Invoke();
        
        
        UpdateTracker.SurpressReload = false;

        
        //todo: make system for editor scripting classes to store data across

        
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
        
        ProjectMemory.RefreshProjectTime(__path);
        
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