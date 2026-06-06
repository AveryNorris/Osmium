namespace OsmiumNucleus;


#nullable enable
public abstract partial class Component
{
    
    
    
    /// <inheritdoc cref="Osmium.Window"/>
    protected static Window OsmiumWindow => Osmium.Window;
    
    
    
    /// <inheritdoc cref="Osmium.DeltaTime"/>
    protected static double DeltaTime => Osmium.DeltaTime;
    
    
    
    /// <inheritdoc cref="Osmium._scenes"/>
    [MarkerAttributes.VariableLambda]
    protected static IReadOnlySet<Scene> Scenes => Osmium._scenes;
    
    
    
    /// <inheritdoc cref="Osmium.AddScene(string)"/>
    [MarkerAttributes.MethodLambda]
    protected static Scene AddScene(string __name) => Osmium.AddScene(__name);
    


    /// <inheritdoc cref="Osmium.GetScene"/>
    [MarkerAttributes.MethodLambda]
    protected static Scene GetScene(string __name) => Osmium.GetScene(__name);
    
    
    
    /// <inheritdoc cref="Osmium.ContainsScene"/>
    [MarkerAttributes.MethodLambda]
    protected static bool ContainsScene(string __name) => Osmium.ContainsScene(__name);



    /// <inheritdoc cref="Osmium.RemoveScene(OsmiumNucleus.Scene)"/>
    [MarkerAttributes.MethodLambda]
    protected static void RemoveScene(Scene __scene) => Osmium.RemoveScene(__scene);



    /// <inheritdock cref="Osmium.RemoveScene(string)" />
    [MarkerAttributes.MethodLambda]
    protected static void RemoveScene(string __name) => Osmium.RemoveScene(__name);
    
    
    
    
    
    /// <inheritdoc cref="ComponentDocker.Move(Component, OsmiumNucleus.ComponentDocker)"/>
    [MarkerAttributes.MethodLambda]
    public void Move(ComponentDocker __newDocker) => Parent.Move(this, __newDocker);



    /// <summary> Makes the Component destroy itself </summary>
    [MarkerAttributes.MethodLambda]
    public void Destroy() => Parent.Destroy(this);



    /// <inheritdoc cref="CoroutineRunner.Start"/>
    [MarkerAttributes.MethodLambda]
    public void Start(IEnumerator<ICoroutineAction> __coroutine) => CoroutineRunner.Start(__coroutine);
    
    
    
}