namespace OsmiumRadium;


/// <summary> Represents an immediate element which is alive for a single frame </summary>
public abstract class ImmediateElement : Element
{
    
    
    
    /// <summary> Is called every frame when the backend enters immediate drawing phase </summary>
    protected internal abstract void Draw();
    
    
    
}