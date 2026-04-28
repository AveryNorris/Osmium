using System.Reflection;
using System.Text.Json.Serialization;
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
    
    public List<string> Tags;
    
    public List<SolidReference> Children = [];

    public Dictionary<string, object?> MemberData = [];

    public static BindingFlags FieldSearchFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static;

    //todo: set value dumb and DUMB DUMB DUMB BREAKS NON PRIMITIVES??

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

        Tags = __component.Tags.ToList();

        foreach (Component child in __component) {
            Children.Add(new SolidReference(child));
        }
        
        //todo: all children unit test
        
        //todo: Make SURE that backing fields are set but dodge property setters
        //todo: make a static value setter in the editor?
        foreach (FieldInfo field in __component.GetType().GetFields(FieldSearchFlags)) {
            MemberData.Add(field.Name, field.GetValue(__component));
        }
    }

    public SolidReference(JsonIntermediate __data) {
        AssemblyName = __data.AssemblyName;
        ComponentTypeName = __data.ComponentTypeName;
        SceneName = __data.SceneName;
        ComponentName = __data.ComponentName;
        Enabled = __data.Enabled;
        Priority = __data.Priority;
        Tags = __data.Tags;

        foreach (JsonIntermediate child in __data.Children) {
            Children.Add(new SolidReference(child));
        }

        foreach (MemberDatum data in __data.MemberData) {
            MemberData.Add(data.FieldName, data.Value);
        } 
    }

    public JsonIntermediate Translate() {
        JsonIntermediate newIntermediate = new JsonIntermediate();
        
        //fix order
        newIntermediate.AssemblyName = AssemblyName;
        newIntermediate.ComponentTypeName = ComponentTypeName;
        newIntermediate.SceneName = SceneName;
        newIntermediate.ComponentName = ComponentName;
        newIntermediate.Enabled = Enabled;
        newIntermediate.Priority = Priority;
        newIntermediate.Tags = Tags;
        
        foreach (SolidReference child in Children) {
            newIntermediate.Children.Add(child.Translate());
        }
        
        //todo: ireadonlyset to ireadonlylist for tags in nucleus?

        foreach (KeyValuePair<string, object?> member in MemberData) {
            newIntermediate.MemberData.Add(new MemberDatum{FieldName = member.Key, Value = member.Value});
        }
        
        return newIntermediate;
    }
    
    //todo: constructor from string (which would be file text) and method to save to file
    

    //doesnt call create
    public Component? Reconstruct(ComponentDocker __parent) {
        

        Assembly? componentAssembly = Context.LoadedProgram!.Assemblies.First(x => x.GetName().Name == AssemblyName);

        if (componentAssembly == null) {
            //todo: add a component map error system and detect assembly/component name refactors
            Debug.LogError("Osmium cannot find a Components assembly! Has its name changed?", ["Assembly Name"], [AssemblyName]);
            return null;
        }
        
        
        //todo: many exceptions
        Type? componentType = componentAssembly.GetType(ComponentTypeName);
        if (componentType == null) {
            Debug.LogError("Osmium cannot find a Component's types! Has its name changed or has it moved assemblies?", ["Component Type"], [ComponentTypeName]);
            return null;
        }
        

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
        
        __parent.Add(newComponent);
        

        foreach (KeyValuePair<string, object?> member in MemberData) {
            
            //todo: maybe cache fields and types to speed up reloading?
            FieldInfo? field = componentType.GetField(member.Key, FieldSearchFlags);

            if (field == null) {
                Debug.LogError("Osmium cannot find a field belonging to a Component!", ["Field Name"], [member.Key]);
                return null;
            }
            
            field.SetValue(newComponent, member.Value);
        }
        
        newComponent.Name = ComponentName;
        newComponent.Enabled = Enabled;
        newComponent.Priority = Priority;
        //todo: figure out why this is surpressive?

        foreach (string tag in Tags) {
            newComponent.AddTag(tag);
        }

        foreach (SolidReference child in Children) {
            child.Reconstruct(newComponent);
        }
        
        return newComponent;
    }
}