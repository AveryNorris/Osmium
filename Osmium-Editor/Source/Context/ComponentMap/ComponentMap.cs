using System.Diagnostics;
using OsmiumNucleus;
using Debug = OsmiumNucleus.Debug;

namespace OsmiumEditor.ComponentMap;

public static partial class ComponentMap
{

    private static List<SolidReference> ComponentReferences = [];
    
    public static bool FirstLoad = true;

    public static bool Opened = false;
    
    //todo: make sure this works first
    public static void Unload() {
        
        
        Debug.Action("Serializing Components!");
        Stopwatch serializeWatch = Stopwatch.StartNew();
        
        ComponentReferences = [];
        
        foreach(Scene scene in Osmium.Scenes) foreach (Component component in scene) {
            ComponentReferences.Add(new SolidReference(component));
        }
        
        serializeWatch.Stop();
        Debug.Action("Serialized Components in " + serializeWatch.ElapsedMilliseconds + "ms!");
    }

    public static void Reload() {
        if(FirstLoad) TryOpenProject();
        FirstLoad = false;
        
        Debug.Action("Reloading Component map!");
        Stopwatch componentMap = Stopwatch.StartNew();
        
        foreach (SolidReference dynamicReference in ComponentReferences) {
            dynamicReference.Build();
        }
        
        componentMap.Stop();
        Debug.Action("Reloaded Component map in " + componentMap.ElapsedMilliseconds + "ms!");
        
        SaveMap();
    }

    public static void TryOpenProject() {
        string ComponentMapPath = Project.GetProjectSubPath(Path.Combine("Editor", "Component.osmap"), regenerate: true);
        string ComponentMap = File.ReadAllText(ComponentMapPath);
        
        foreach (string jsonSegment in ComponentMap.Split('#')) {
            if (jsonSegment == string.Empty) return;

            SolidReference solidReference = SolidReference.Parse(jsonSegment);
            solidReference.Build();
            
            ComponentReferences.Add(solidReference);
        }
        //todo: make this assume scene map is correct and throw correcting errors if not
    }

    public static void SaveMap() {
        Debug.Action("Writing Serialized Components!");
        
        string ComponentMap = string.Empty;
        foreach (SolidReference dynamicReference in ComponentReferences) {
            ComponentMap += dynamicReference.CreateJson() + '#';
        }
        
        Stopwatch saveWatch = Stopwatch.StartNew();
        
        string ComponentMapPath = Project.GetProjectSubPath(Path.Combine("Editor", "Component.osmap"), regenerate: true);
        File.WriteAllText(ComponentMapPath, ComponentMap);
        //todo: always use get path in case things are removed mid runtime and rename to getandregenpath() or something
        
        saveWatch.Stop();
        Debug.Action("Compiled and wrote Component Map in " + saveWatch.ElapsedMilliseconds + "ms!");
    }
}