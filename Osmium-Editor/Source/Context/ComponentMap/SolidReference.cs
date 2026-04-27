using System.Reflection;
using OsmiumNucleus;


namespace OsmiumEditor.ComponentMap;



/// <summary> Represents a reference to a Component that is reload infallible. In other words it reconstructs the Component after the reload </summary>
public class SolidReference
{

    
    //todo: add a way to detect refactors, and move down sequentially for instance
    //if an assembly name change is found fix that first, and then recheck for errors, then namespace etc
    
    public string AssemblyName;
    public string ComponentTypeName;

    public string SceneName;
    public string ComponentName;
    
    public bool Enabled;
    public int Priority;
    
    public IReadOnlySet<string> Tags;

    public Dictionary<string, object?> MemberData = [];

    public static BindingFlags FieldSearchFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;


    //todo: directly save order profile and enable internal visualization in osmium-nucleus

    public SolidReference(Component __component) {
        
        //todo: so many exceptions ;(
        AssemblyName = __component.GetType().Assembly.GetName().Name!;
        ComponentTypeName = __component.GetType().FullName!;
        
        //todo: nucleus exceptions
        
        //todo: fix reference type issues
        SceneName = __component.Scene!.Name;
        ComponentName = __component.Name;
        
        Enabled = __component.Enabled;
        Priority = __component.Priority;

        Tags = __component.Tags;
        
        //todo: all children unit test
        
        //todo: Make SURE that backing fields are set but dodge property setters
        //todo: make a static value setter in the editor?
        foreach (FieldInfo field in __component.GetType().GetFields(FieldSearchFlags)) {
            MemberData.Add(field.Name, field.GetValue(__component));
        }
    }
    
    //todo: constructor from string (which would be file text) and method to save to file
    

    //doesnt call create
    public Component? Reconstruct() {
        
        Debug.LogAction("1");

        Assembly? componentAssembly = Context.LoadedProgram!.Assemblies.First(x => x.GetName().Name == AssemblyName);

        if (componentAssembly == null) {
            //todo: add a component map error system and detect assembly/component name refactors
            Debug.LogError("Osmium cannot find a Components assembly! Has its name changed?", ["Assembly Name"], [AssemblyName]);
            return null;
        }
        
        Debug.LogAction("2");
        
        //todo: many exceptions
        Type? componentType = componentAssembly.GetType(ComponentTypeName);
        if (componentType == null) {
            Debug.LogError("Osmium cannot find a Component's types! Has its name changed or has it moved assemblies?", ["Component Type"], [ComponentTypeName]);
            return null;
        }
        
        Debug.LogAction("3");

        Component newComponent;
        try {
            Component? instantiatedComponent = Activator.CreateInstance(componentType) as Component;

            //todo: clean up here  and move out of try catch cuz it breaks if debug is set to throw exceptions
            if (instantiatedComponent == null) {
                Debug.LogError("Osmium failed to create an instance of a Component!");
                return null;
            }
            
            newComponent = instantiatedComponent;
        } catch(Exception e) {
            Debug.LogError("Reinstantiating a referenced Component has failed!", ["Error"], [e.Message]);
            return null;
        }
        
        Debug.LogAction("4");

        foreach (KeyValuePair<string, object?> member in MemberData) {
            
            //todo: maybe cache fields and types to speed up reloading?
            FieldInfo? field = componentType.GetField(member.Key, FieldSearchFlags);

            if (field == null) {
                Debug.LogError("Osmium cannot find a field belonging to a Component!", ["Field Name"], [member.Key]);
                return null;
            }
            
            field.SetValue(newComponent, member.Value);
        }
        
        Debug.LogAction("5");

        newComponent.Name = ComponentName; 
        newComponent.Enabled = Enabled; 
        newComponent.Priority = Priority;

        Debug.LogAction("a");
        foreach (string tag in Tags) {
            newComponent.AddTag(tag);
            Debug.LogAction("c");
        }
        Debug.LogAction("b");
        
        Debug.LogAction("6");

        Scene scene = Osmium.AddScene(SceneName) ?? Osmium.GetScene(SceneName)!;
        
        scene.Add(newComponent);

        return newComponent;
    }
}