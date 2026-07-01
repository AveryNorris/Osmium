

namespace OsmiumNucleus;

/// <summary>One of the main Osmium entities. Acts like a master folder for components to be stored in. Feel free to inherit this and make a custom scene! </summary>
/// <author> Avery Norris </author>
public class Scene(string __name) : ComponentDocker
{

    
    
    /// <summary> Whether the Scene is capturing events or not </summary>
    public bool Enabled = true;



    /// <summary> Unique identifier of the Scene</summary>
    public string Name
    {
        get;
        set
        {
            if (value == field) return;
            if (!Osmium.ContainsScene(value)) field = value;
            else Debug.Error("A scene with this name already exists!", ["Name"], [value]);
        }
    } = __name;

    
    

    
}