using System.Collections.Frozen;
using System.Reflection;
using OsmiumBedrock;

namespace OsmiumNucleus;

public static partial class Osmium
{
    /// <summary> Initializes the Context and marks Osmium as initialized; but does not Resolve types </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// These methods are made required in order to use Virtualization! Use Editor Methods instead of normal ones for Virtualization to work.</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void EditorInitialize() {
        Debug.Log("Successfully Initialized Osmium!");
    }
    
    
    
    /// <summary> Starts OpenTK but doesn't let the update loop run! </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// These methods are made required in order to use Virtualization! Use Editor Methods instead of normal ones for Virtualization to work.</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void EditorRun() {
        Bedrock.Update += OnUpdate;
        Bedrock.Draw += OnDraw;
        
        Window!.Run();
    }
    
    
    
    /// <summary> Pretends to initialize Osmium, and makes the Components think that the Game has just been initialized. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void VirtualInitialize(IEnumerable<Assembly> __assemblies) {
        EventManager._TypeAssociatedTimeEvents = FrozenDictionary<Type, EventManager.EventProfile>.Empty;
        EventManager.OnInitializeEvents.Clear();
        IsInitialized = true;
        IsVirtualized = true;
        
        EventManager.ResolveAllModules(__assemblies);
        EventManager.InvokeModulesInitializeEvent();
    }
    
    
    
    /// <summary> Pretends to run Osmium virtually, and makes the Components think it has Started. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for Radium! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void VirtualRun() {
        IsRunning = true;
        
        foreach (Scene scene in Scenes) scene.ChainEvent(Event.Load); 
    }
    
    
    
    /// <summary> Pretends to close Osmium, and makes the Components think that the Game has ended. </summary>
    /// <remarks> This is part of the Editor pipeline! It has no error checking, and it is made explicitly for the Editor! So don't use it unless you know what you are doing.
    /// If you do want to use it, use the EditorInitialize() EditorRun() and EditorClose() instead of the traditional methods!</remarks>
    [MarkerAttributes.UnsafePipeline]
    public static void VirtualClose() {
        if (IsRunning) {
            foreach (Scene scene in Scenes) scene.ChainEvent(Event.Unload);
            IsRunning = false;
        }
        
        IsVirtualized = false;
    }
}