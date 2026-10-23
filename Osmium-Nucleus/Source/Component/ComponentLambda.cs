using OsmiumBedrock;

namespace OsmiumNucleus;


#nullable enable
public abstract partial class GameObject
{
    
    
    
    /// <inheritdoc cref="Osmium.DeltaTime"/>
    protected static double DeltaTime => Osmium.DeltaTime;
    
    
    
    /// <inheritdoc cref="Osmium._scenes"/>
    protected static IReadOnlySet<Scene> Scenes => Osmium._scenes;
    
    
    
    /// <inheritdoc cref="Osmium.AddScene(string)"/>
    protected static Scene AddScene(string __name) => Osmium.AddScene(__name);
    


    /// <inheritdoc cref="Osmium.GetScene"/>
    protected static Scene GetScene(string __name) => Osmium.GetScene(__name);
    
    
    
    /// <inheritdoc cref="Osmium.ContainsScene"/>
    protected static bool ContainsScene(string __name) => Osmium.ContainsScene(__name);



    /// <inheritdoc cref="Osmium.RemoveScene(OsmiumNucleus.Scene)"/>
    protected static void RemoveScene(Scene __scene) => Osmium.RemoveScene(__scene);



    /// <inheritdock cref="Osmium.RemoveScene(string)" />
    protected static void RemoveScene(string __name) => Osmium.RemoveScene(__name);
    
    
    
    
    
    /// <inheritdoc cref="Entity.Move(CoEntityentDocker)"/>
    public void Move(Entity __newDocker) => Docker.Move(this, __newDocker);



    /// <summary> Makes the GameObject destroy itself </summary>
    public void Suicide() => Docker.Destroy(this);
    
    public void Destroy(GameObject gameObject) => gameObject.Docker.Destroy(gameObject);



    /// <inheritdoc cref="CoroutineRunner.Start"/>
    public void Start(IEnumerator<ICoroutineAction> __coroutine) => CoroutineRunner.Start(__coroutine);
    
    
    
}