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
        
        Debug.Action("Reloading Osmium!");
        Stopwatch timer = Stopwatch.StartNew();
        
        
        IsReloading = true;
        
        
        IsUnloading = true;
        Debug.Clear();
        OnUnload?.Invoke();
        IsUnloading = false;
        
        
        IsLoading = true;
        OnLoad?.Invoke();
        IsLoading = false;
        
        
        IsReloading = false;
        
        timer.Stop();
        Debug.Action("Osmium finished reloading in: " + timer.Elapsed.Milliseconds + "ms!");
    }
}