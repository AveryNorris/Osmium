using Dear_ImGui_Sample.Backends;
using OsmiumNucleus;

namespace OsmiumEditor.Source.DearImGUINET.Structure;

public static class EditorWindowHierarchy
{

    static EditorWindowHierarchy() {
        Bedrock.Update += Update;
        Bedrock.Draw += Draw;
        Bedrock.Unload += Unload;
    }
    
    public static readonly HashSet<EditorWindow> CurrentWindows = [];

    public static void Add(EditorWindow window) {
        CurrentWindows.Add(window);
        
        window.Create();
    }

    public static void Add<T>() where T : EditorWindow, new() {
        Add(new T());
    }

    public static void Remove(EditorWindow window) {
        window.Destroy();
        
        CurrentWindows.Remove(window);
    }

    public static void Remove<T>() where T : EditorWindow {
        EditorWindow? window = CurrentWindows.FirstOrDefault(w => w is T);

        if (window == null) return;
        
        Remove(window);
    }
    
    public static void Unload() { foreach (EditorWindow window in CurrentWindows) window.Unload(); }

    public static void Update() { foreach (EditorWindow window in CurrentWindows) window.Update(); }
    public static void Draw() { foreach (EditorWindow window in CurrentWindows) window.Draw(); }
    
    
}