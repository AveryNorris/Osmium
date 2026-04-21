namespace OsmiumNucleus;


#nullable enable
public abstract partial class Component
{
    
    
    
    /// <inheritdoc cref="Osmium.Context"/>
    protected static Context OsmiumContext => Osmium.Context;
    
    
    
    /// <inheritdoc cref="Osmium.DeltaTime"/>
    protected static double DeltaTime => Osmium.DeltaTime;
    
    
    
    /// <inheritdoc cref="Osmium._scenes"/>
    [MarkerAttributes.VariablePointer]
    protected static IReadOnlySet<Scene> Scenes => Osmium._scenes;
    
    
    
    /// <inheritdoc cref="Osmium.AddScene(string)"/>
    [MarkerAttributes.MethodPointer]
    protected static Scene AddScene(string __name) => Osmium.AddScene(__name);
    


    /// <inheritdoc cref="Osmium.GetScene"/>
    [MarkerAttributes.MethodPointer]
    protected static Scene GetScene(string __name) => Osmium.GetScene(__name);
    
    
    
    /// <inheritdoc cref="Osmium.ContainsScene"/>
    [MarkerAttributes.MethodPointer]
    protected static bool ContainsScene(string __name) => Osmium.ContainsScene(__name);



    /// <inheritdoc cref="Osmium.RemoveScene(OsmiumNucleus.Scene)"/>
    [MarkerAttributes.MethodPointer]
    protected static void RemoveScene(Scene __scene) => Osmium.RemoveScene(__scene);



    /// <inheritdock cref="Osmium.RemoveScene(string)" />
    [MarkerAttributes.MethodPointer]
    protected static void RemoveScene(string __name) => Osmium.RemoveScene(__name);
    
    
    
    
    
    /// <inheritdoc cref="ComponentDocker.Move(Component, OsmiumNucleus.ComponentDocker)"/>
    [MarkerAttributes.MethodPointer]
    public void Move(ComponentDocker __newDocker) => Parent.Move(this, __newDocker);



    /// <summary> Makes the Component destroy itself </summary>
    [MarkerAttributes.MethodPointer]
    public void Destroy() => Parent.Destroy(this);
}