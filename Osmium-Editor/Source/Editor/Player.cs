using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{

    public static event Action? OnRun;
    public static event Action? OnEnd;
    
    public static void RunGame() {
        if (Osmium.IsRunning) return;
        
        EditorCompile();
        
        Osmium.VirtualInitialize([..RuntimeAssemblies, ..EditorLoadContext.Assemblies]);
        
        Osmium.VirtualRun();
        
        OnRun?.Invoke();
    }

    public static void CloseGame() {
        if (!Osmium.IsRunning) return;
        
        OnEnd?.Invoke();
        
        Osmium.VirtualClose();
        
        EditorLoadContext.Unload();
    }
}