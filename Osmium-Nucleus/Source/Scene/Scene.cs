

namespace OsmiumNucleus;

/// <summary> Represents the root in a tree of <see cref="GameObject">GameObjects</see>. </summary>
/// <param name="Name"> The unique identifier of the Scene </param>
public class Scene(string Name) : Entity
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