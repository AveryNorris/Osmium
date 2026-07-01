using System.Reflection;
using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor.Serialization;

/// <summary> The Map is responsible for serialization and ordering of Components and Scenes. </summary>
public static class Map
{

    public const string FileWarningHeader = "==================================\nWARNING : THIS FILE IS EXTREMELY IMPORTANT!\n==================================\n\nYour project will lose all saved Component, Scenes and Serialized members if this is \nremoved. Messing with it is avoided; unless you know what you are doing!\n\n----------------------------------\n\n";
    
    public static readonly List<SceneMapNode> Scenes = [];

    static Map() {
        Osmium.SceneAdded += OnSceneAdded;
        Osmium.SceneRemoved += OnSceneRemoved;
        
        ComponentDocker.ComponentAdded += OnComponentAdded;
        ComponentDocker.ComponentRemoved += OnComponentRemoved;
        ComponentDocker.ComponentMoved += OnComponentMoved;

        Context.OnUnload += OnUnload;
        Context.OnReload += OnReload;
    }

    public static DockerMapNode? FindMappedDocker(ComponentDocker __targetDocker) {
        List<DockerMapNode> TargetDockers = [];
        TargetDockers.AddRange(Scenes.Select(x => (DockerMapNode) x));

        for (int i = 0; i < TargetDockers.Count; i++)
        {
            DockerMapNode CurrentMappedDocker = TargetDockers[i];

            if (CurrentMappedDocker.docker == __targetDocker) {
                return CurrentMappedDocker;
            }
            
            TargetDockers.AddRange(CurrentMappedDocker.Children);
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
    
    public static SceneMapNode? FindMappedScene(Scene __targetScene) {
        foreach (SceneMapNode CurrentMappedScene in Scenes)
        {
            if (CurrentMappedScene.scene == __targetScene)
            {
                return CurrentMappedScene;
            }
        }

        return null;
    }

    public static void OnSceneAdded(Scene scene) {
        Scenes.Add(new SceneMapNode(scene));
    }

    public static void OnSceneRemoved(Scene scene) {
        foreach (SceneMapNode mappedScene in Scenes.ToArray()) 
            if(mappedScene.scene == scene) Scenes.Remove(mappedScene); 
    }

    public static void OnComponentAdded(ComponentDocker docker, Component component) {
         FindMappedDocker(docker).Children.Add(new ComponentMapNode(component));
    }

    public static void OnComponentRemoved(ComponentDocker docker, Component component) {
        FindMappedDocker(docker).Children.RemoveAll(x => x.component == component);
    }

    public static void OnComponentMoved(ComponentDocker original, ComponentDocker newDocker, Component component) {
        ComponentMapNode componentMapNode = FindMappedComponent(component);

        FindMappedDocker(original).Children.Remove(componentMapNode);
        FindMappedDocker(newDocker).Children.Add(componentMapNode);
    }

    public static void OnUnload() {
        foreach (SceneMapNode sceneMapNode in Scenes)
        {
            sceneMapNode.Unload();
        }
    }
    
    public static void OnReload() {
        foreach (SceneMapNode sceneMapNode in Scenes)
        {
            sceneMapNode.Construct();
        }
    }

    public static void Save() {
        string writeValue = FileWarningHeader + "\n^";

        foreach (SceneMapNode mappedScene in Scenes)
        {
            writeValue += "\n" + mappedScene.scene.GetType().FullName + CompressDockerData(mappedScene);
        }

        List<ComponentMapNode> TargetComponents = Scenes.SelectMany(x => x.Children).ToList();

        foreach (ComponentMapNode componentMapNode in TargetComponents.ToArray()) {
            writeValue += "\n ";
            
            for (int i = 0; i < componentMapNode.component.AllParents.Count; i++) {
                writeValue += ' ';
            }
            
            writeValue += componentMapNode.component.GetType().FullName + CompressDockerData(componentMapNode);
            
            TargetComponents.AddRange(componentMapNode.Children);
        }

        File.WriteAllText(Project.GetProjectSubdirectory(Path.Combine("Editor", "Osmium.osmap")), writeValue);
    }

    public static void Load() {
        string[] saveData = File.ReadAllText(Project.GetProjectSubdirectory(Path.Combine("Editor", "Osmium.osmap"), true)).Split("^");

        if (saveData.Length == 0)
        {
            Debug.Log("Invalid or NonPresent save data!");
            return;
        }

        string text = saveData[^1];

        List<DockerMapNode> GeneratedTree = [];

        foreach (string member in text.Split("\n"))
        {
            if (member == string.Empty)
                continue;
            
            int depth = 0;
            
            for (int i = 0; i < member.Length; i++)
            {
                if (member[i] == ' ')
                    depth++;
                else break;
            }

            foreach (DockerMapNode node in GeneratedTree.ToArray())
            {
                if (GeneratedTree.IndexOf(node) >= depth)
                {
                    GeneratedTree.Remove(node);
                }
            }

            DockerMapNode newDocker;
            
            string[] data = member.Substring(depth).Split("#");
            if (data.Length == 0)
            {
                Debug.Error("Corrupt save data!");
                continue;
            }
            
            //todo: add try catches to events for all plugins, and the game running, and turn off the map when the game is running.
            
            Type? foundType = null;

            foreach (Assembly assembly in Context.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    Debug.Log(type.FullName);
                    if (type.FullName == data[0])
                    {
                        foundType = type;
                    }
                }
            }
            
            if (foundType == null)
            {
                Debug.Error("Failure! Failed to find a defined type", ["TypeName"], [data[0]]);
                continue;
            }
            
            //todo: totally assumes that the type is a scene or component based on depth, double check that! and make sure there is no tricky buisness with
            //todo: custom component or scenes that inherit one or the other
            if (depth == 0)
            {
                //todo: make a class that verifies that save data is not corrupted, no matching names etc

                //todo: can things inherit scenes avoid a constructor with just string?
                Scene scene = Activator.CreateInstance(foundType, "MAP_GENERATED_SCENE") as Scene;

                Osmium.AddScene(scene);
                    
                newDocker = FindMappedScene(scene);
            }
            else
            {
                Component component = Activator.CreateInstance(foundType) as Component;
                
                GeneratedTree[^1].docker.Add(component);
                
                newDocker = FindMappedComponent(component);
            }

            if (newDocker == null)
                return;

            for (int i = 1; i < data.Length; i++)
            {
                SerializedVariable? newField = SerializedVariable.FromSaveData(foundType, data[i]);

                if (newField != null && newField.IsValid)
                {
                    newDocker.SerializedFields.Add(newField);

                    newField.Load(newDocker);
                }
            }
            
            GeneratedTree.Add(newDocker);
        }
    }

    public static string CompressDockerData(DockerMapNode docker) {
        string returnValue = string.Empty;
        
        foreach (SerializedVariable dockerDatum in docker.SerializedFields) {
            returnValue += "#" + dockerDatum.ToSaveData();
            //todo: constrain to save data
        }

        return returnValue;
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
        
        DockerMapNode node = FindMappedDocker(docker)!;
        
        node.SerializedFields.RemoveAll(x => SerializedVariable.SameTarget(serializedVariable, x));
            
        //todo: add support, if it is the default value of the field remove the saved data instead
        node.SerializedFields.Add(serializedVariable);
        
        serializedVariable.Load(docker);
    }

}

public class SceneMapNode(Scene scene) : DockerMapNode(scene) {
    public Scene? scene = scene;
    public SolidType? reloadMemoryType = new SolidType(scene);

    public static implicit operator Scene(SceneMapNode node) => node.scene;

    public void Unload() {
        scene = null;
        docker = null;
        IsLoaded = false;

        foreach (ComponentMapNode component in Children) {
            component.Unload();
        }
    }

    public void Construct() {
        scene = (Scene)Activator.CreateInstance(reloadMemoryType.FindType(), "MAP_GENERATED_SCENE");
        Osmium.AddScene(scene);
        
        docker = scene;

        foreach (SerializedVariable variable in SerializedFields) variable.Load(docker);
        
        IsLoaded = true;

        foreach (ComponentMapNode child in Children) child.Construct(this);
    }
}

public class ComponentMapNode(Component component) : DockerMapNode(component) {
    public Component component = component;
    public SolidType? reloadMemoryType = new SolidType(component);
    
    public static implicit operator Component(ComponentMapNode node) => node.component;
    
    public void Unload() {
        component = null;
        docker = null;
        IsLoaded = false;
    }

    public void Construct(DockerMapNode parent) {
        component = (Component)Activator.CreateInstance(reloadMemoryType.FindType());
        parent.docker.Add(component);
        
        docker = component;

        foreach (SerializedVariable variable in SerializedFields) variable.Load(docker);
        
        IsLoaded = true;

        foreach (ComponentMapNode child in Children) child.Construct(this);
    }
}

public abstract class DockerMapNode(ComponentDocker docker)
{
    public bool IsLoaded = true;
    
    public ComponentDocker? docker = docker;
    public List<ComponentMapNode> Children = docker.Children.Select(x => new ComponentMapNode(x)).ToList();
    
    public List<SerializedVariable> SerializedFields = [];
    
    public static implicit operator ComponentDocker(DockerMapNode node) => node.docker;

}