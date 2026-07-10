using OsmiumEditor.Source.NewEditor.Serialization;
using OsmiumNucleus;

namespace OsmiumEditor;

/// <summary> Represents a node in the Component Map that points to a Component </summary>
public sealed class ComponentMapNode : SerializedDocker {


    
    /// <summary> Creates a new Map Node from a Component </summary>
    /// <param name="__component"></param>
    internal ComponentMapNode(Component __component) : base(__component) {
        component = __component;
        reloadMemoryType = new SolidType(__component);
    }
    
    
    
    /// <summary> The original Component this serialized Component points to; will be null if the Editor is currently unloading and reloading! </summary>
    public Component component { get; private set; }
    
    /// <summary> The Component's type </summary>
    public readonly SolidType reloadMemoryType;
    
    
    
    /// <summary> Unloads all Context data </summary>
    public void Unload() {
        component = null;
        docker = null;
    }

    
    
    /// <summary> Instantiates the Component and Serializes all fields  </summary>
    /// <param name="parent"></param>
    public void Construct(SerializedDocker parent) {
        component = (Component) Activator.CreateInstance(reloadMemoryType.FindType()!)!;
        parent.docker!.Add(component);
        
        docker = component;

        foreach (SerializedVariable variable in SerializedFields) variable.Load(docker);
        
        foreach (ComponentMapNode child in Children) child.Construct(this);
    }
}