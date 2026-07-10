using System.Reflection;
using OsmiumEditor.Source;
using OsmiumEditor.Source.NewEditor.Serialization;
using OsmiumNucleus;

namespace OsmiumEditor;

/// <summary> The Map is responsible for serialization and ordering of Components and Scenes. </summary>
public static partial class DockerMap
{

    /// <summary> A warning header that appears in files instructing not to delete this before the Map information, be sure not to use a caret in the header!</summary>
    public const string FileWarningHeader = "==================================\nWARNING : THIS FILE IS EXTREMELY IMPORTANT!\n==================================\n\nYour project will lose all saved Component, Scenes and Serialized members if this is \nremoved. Messing with it is avoided; unless you know what you are doing!\n\n----------------------------------\n\n";

    /// <summary> The target local file path of the Osmium Map </summary>
    public static readonly string[] FilePath = ["Editor",  "Osmium.osmap"];
    
    
    
    /// <summary> The top of the map, all the Scenes present. </summary>
    public static readonly List<SerializedScene> Scenes = [];

    
    
    static DockerMap() {
        Osmium.SceneAdded += OnSceneAdded;
        Osmium.SceneRemoved += OnSceneRemoved;
        
        ComponentDocker.ComponentAdded += OnComponentAdded;
        ComponentDocker.ComponentRemoved += OnComponentRemoved;
        ComponentDocker.ComponentMoved += OnComponentMoved;

        Context.OnUnload += UnloadMapReferences;
        Context.OnReload += ReconstructMapReferences;

        Editor.OnSave += Save;
        
        Osmium.LoadInitializer += UnloadMapReferences;
        
        //todo: might be unnecessary if reload is called after closing Osmium
        Osmium.UnloadFinalizer += ReconstructMapReferences;
    }

    public static SerializedDocker? FindMappedDocker(ComponentDocker __targetDocker) {
        List<SerializedDocker> TargetDockers = [];
        TargetDockers.AddRange(Scenes.Select(x => (SerializedDocker) x));

        for (int i = 0; i < TargetDockers.Count; i++)
        {
            SerializedDocker currentMappedSerializedDocker = TargetDockers[i];

            if (currentMappedSerializedDocker.docker == __targetDocker) {
                return currentMappedSerializedDocker;
            }
            
            TargetDockers.AddRange(currentMappedSerializedDocker.Children);
        }

        return null;
    }
    
    public static ComponentMapNode? FindMappedComponent(Component __targetComponent) {
        List<ComponentMapNode> TargetDockers = [];
        TargetDockers.AddRange(Scenes.SelectMany(x => x.Children));

        for (int i = 0; i < TargetDockers.Count; i++)
        {
            ComponentMapNode currentDocker = TargetDockers[i];

            if (currentDocker.component == __targetComponent) {
                return currentDocker;
            }
            
            TargetDockers.AddRange(currentDocker.Children);
        }

        return null;
    }
    
    public static SerializedScene? FindMappedScene(Scene __targetScene) {
        foreach (SerializedScene CurrentMappedScene in Scenes)
        {
            if (CurrentMappedScene.scene == __targetScene)
            {
                return CurrentMappedScene;
            }
        }

        return null;
    }

    public static void OnSceneAdded(Scene scene) {
        if (Osmium.IsRunning) return;
        Scenes.Add(new SerializedScene(scene));
    }

    public static void OnSceneRemoved(Scene scene) {
        if (Osmium.IsRunning) return;
        foreach (SerializedScene mappedScene in Scenes.ToArray()) 
            if(mappedScene.scene == scene) Scenes.Remove(mappedScene); 
    }

    public static void OnComponentAdded(ComponentDocker docker, Component component) {
        if (Osmium.IsRunning) return;
        FindMappedDocker(docker).Children.Add(new ComponentMapNode(component));
    }

    public static void OnComponentRemoved(ComponentDocker docker, Component component) {
        if (Osmium.IsRunning) return;
        FindMappedDocker(docker).Children.RemoveAll(x => x.component == component);
    }

    public static void OnComponentMoved(ComponentDocker original, ComponentDocker newDocker, Component component) {
        if (Osmium.IsRunning) return;
        
        ComponentMapNode componentMapNode = FindMappedComponent(component);

        FindMappedDocker(original).Children.Remove(componentMapNode);
        FindMappedDocker(newDocker).Children.Add(componentMapNode);
    }

    public static void UnloadMapReferences() {
        foreach (SerializedScene sceneMapNode in Scenes)
        {
            sceneMapNode.Unload();
        }
    }
    
    public static void ReconstructMapReferences() {
        foreach (SerializedScene sceneMapNode in Scenes)
        {
            sceneMapNode.Construct();
        }
    }

    

    public static void SetVariable(ComponentDocker docker, string fieldName, object? value) {
        
        MemberInfo? memberInfo = docker.GetType().GetField(fieldName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

        if (memberInfo == null)
        {
            memberInfo = docker.GetType().GetProperty(fieldName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

            if (memberInfo == null)
            {
                //todo: debug.Fatal
                Debug.Error("Failed to find a field or property! ", ["ClassName", "FieldName"], [docker.GetType().Name, fieldName]);
            }
        }

        SerializedVariable serializedVariable = new SerializedVariable(new SolidMember(memberInfo), value);

        if (!serializedVariable.IsValid) {
            Debug.Error("Serialized Variable is invalid! Terminating operation...");
            return;
        }
        
        SerializedDocker node = FindMappedDocker(docker)!;
        
        serializedVariable.Load(docker);
        
        if (Osmium.IsRunning) return;
        
        node.SerializedFields.RemoveAll(x => SerializedVariable.SameTarget(serializedVariable, x));
            
        //todo: add support, if it is the default value of the field remove the saved data instead
        node.SerializedFields.Add(serializedVariable);
    }
    
    //todo: make scenes try events like Components

}
