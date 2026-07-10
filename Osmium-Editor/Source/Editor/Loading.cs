using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using OsmiumNucleus;
using Debug = OsmiumNucleus.Debug;

namespace OsmiumEditor;

public static partial class Editor
{
    
    //todo: try catch all of this shit, 

    public static event Action? OnSave;

    public static event Action? OnUnload;
    public static event Action? OnUnloadFinalizer;
    
    public static event Action? OnLoadInitializer;
    public static event Action? OnLoad;

    public static bool IsUnloading { get; private set; }
    public static bool IsLoading { get; private set; }
    public static bool IsReloading { get; private set; }
    
    public static void Save() {
        OnSave?.Invoke();
    }

    public static void Reload() {
        if (Osmium.IsRunning) {
            Debug.Error("You cannot reload when Osmium is running!");
            return;
        }
        
        if (Osmium.IsInitialized) {
            Osmium.VirtualClose();
        }
        
        
        
        Debug.Action("Reloading Osmium!");
        Stopwatch timer = Stopwatch.StartNew();
        
        
        IsReloading = true;
        
        
        Unload();
        
        
        Load();
        
        
        IsReloading = false;
        
        timer.Stop();
        Debug.Action("Osmium finished reloading in: " + timer.Elapsed.Milliseconds + "ms!");
    }

    private static void Unload() {
        IsUnloading = true;
        
        

        OnUnload?.Invoke();
        _RuntimeModules.Unload();
        _RuntimeModules = new AssemblyLoadContext(null, true);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        OnUnloadFinalizer?.Invoke();
        
        
        
        IsUnloading = false;
    }

    private static void Load() {
        IsLoading = true;
        

        
        OnLoadInitializer?.Invoke();
        foreach (string runtimeModule in Directory.GetFiles(Project.GetProjectSubdirectory(true, "Modules", "Runtime"), "*.dll", SearchOption.AllDirectories))
            _RuntimeModules.LoadFromAssemblyPath(runtimeModule);
        Osmium.VirtualInitialize(_RuntimeModules.Assemblies);
        OnLoad?.Invoke();

        
        
        IsLoading = false;
    }
}