

namespace OsmiumNucleus;

/// <summary> The root for a hierarchy of <see cref="Component">Components</see>. Scenes are stored with a list inside of <see cref="Osmium"/>, and <see cref="Component">Components</see> are added to said Scene.</summary>
/// <param name="Name"> The fully unique identifier of the Scene. Must not match a name from any other Scene. And cannot be null. </param>
public class Scene(string Name) : ComponentDocker
{
    
    

    /// <summary> The fully unique identifier of the <see cref="Scene"/>. Must not match a name from any other <see cref="Scene"/>. And cannot be null.  </summary>
    public string Name
    {
        get;
        set {
            if (value == field) return;
            if (value == null) { Debug.Error("You cannot give a Scene a null name!"); return; }
            if (Osmium.ContainsScene(value)) { Debug.Error($"A scene with the name \"{value}\" already exists!"); return; }
            
            field = value;
        }
    } = Name;

    
    

    
}