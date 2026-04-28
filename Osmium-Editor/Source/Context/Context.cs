
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

    public static bool FirstLoad = true;
    
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
        //todo: make method
        UpdateTracker.SurpressReload = true;
        MemoryStream assemblyStream = ScriptCompiler.CompileScripts();

        if (!ScriptCompiler.Success)  { Debug.LogError("Source did not compile! "); return; }


        List<SolidReference> Components = [];
        List<string> Scenes = [];

        if (!FirstLoad) {
            foreach (Scene scene in Osmium.Scenes) {

                Scenes.Add(scene.Name);

                foreach (Component component in scene.Children) {

                    Debug.LogAction("saving component");

                    Components.Add(new SolidReference(component));
                    component.Destroy();
                }

                //destroy scene change this! and make it attach to scene
                Osmium.RemoveScene(scene);
            }
        }

        Stopwatch timer = Stopwatch.StartNew();
        
        Debug.LogAction("Reloading Osmium!");
        

        //keep old scripts if new ones do not compiles

        OnUnload?.Invoke();



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
        
        if (FirstLoad) {
            
            //todo: store scene data
            foreach (string referenceData in File.ReadAllText(ComponentMapPath).Split('#')) {
            
                //todo: fix debugs in event resolved to use parameters and not concatenation and debug.log
                Debug.LogAction(referenceData);
            
                if (referenceData == string.Empty) continue;
            
                //todo: streamline in solid reference and rename solid reference class
                JsonIntermediate jsonIntermediate = System.Text.Json.JsonSerializer.Deserialize<JsonIntermediate>(referenceData);
            
                SolidReference newReference = new SolidReference(jsonIntermediate);
                newReference.Reconstruct(Osmium.GetScene(newReference.SceneName) ?? Osmium.AddScene(newReference.SceneName));
            }
        }
        

        foreach (string sceneName in Scenes) {
            Osmium.AddScene(sceneName);
        }

        string OsmiumMap = string.Empty;
        
        foreach (SolidReference component in Components) {

            OsmiumMap += System.Text.Json.JsonSerializer.Serialize(component.Translate()) + '#';

            component.Reconstruct(Osmium.GetScene(component.SceneName) ?? Osmium.AddScene(component.SceneName));
        }
        
        File.WriteAllText(ComponentMapPath, OsmiumMap);
        
        OnReload?.Invoke();

        UpdateTracker.SurpressReload = false;
        
        timer.Stop();
        Debug.LogAction("Reload finished in: " + timer.Elapsed.Milliseconds + "ms!");

        //todo: wait to reload if a game is running
    }

    //assumes a project was opened from the select menu
    //todo: move all to editor?
    public static void OpenProject(string __path)
    {
        
        //todo: event
        
        Debug.LogAction("Opening project! ", ["Path"], [__path]);
        
        //check for valid project
        string parentDirectory = Path.GetDirectoryName(__path);
        
        Project.ProjectPath = parentDirectory;
        
        UpdateTracker.Initialize();
        
        ComponentMapPath = Project.GetProjectSubPath(Path.Combine("Editor", "Component.osmap"), regenerate: true);
        UpdateTracker.BlacklistedPaths.Add(ComponentMapPath);
        
        Reload();
        FirstLoad = false;
        
        //post reload so the assemblies exist
        
        //todo: check file permissions and throw in the editor if we dont have
    }
}