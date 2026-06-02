namespace OsmiumNucleus;

/// <summary> Manages external libraries and allows them to seamlessly hook into Osmium </summary>
public interface IModule
{
    
    public void Initialize() {}
    
    public void Run() {}

    public void Close() {}
    
}