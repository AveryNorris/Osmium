namespace OsmiumNucleus;



/// <summary> Bottom class of Osmium. Carries events from MonoGame into Scenes, and provides OpenTK context.</summary>
/// <author> Avery Norris </author>
public static partial class Osmium
{
    
    
    
    
    
    /// <summary> An event that is raised before all load calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? FirstLoad;

    public static event Action? Load;
    /// <summary> An event that is raised after all load calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? FinalLoad;

    
    
    /// <summary> OnLoad() is called by OpenTK when the program starts; Calls an event called Load() in Components</summary>
    /// <remarks> It is recommended to load content during Load()</remarks>
    private static void OnLoad() {
        try {
            FirstLoad?.Invoke();

            foreach (Scene scene in _scenes) if (scene.Enabled) scene.ChainEvent(0);
            Load?.Invoke();

            FinalLoad?.Invoke();
        }catch (Exception e) {
            TrySafeEscape(e);
        }
    }



    
    
    /// <summary> An event that is raised before all unload calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? FirstUnload;
    
    public static event Action? Unload;
    /// <summary> An event that is raised after all unload calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? FinalUnload;
    
    /// <summary> OnClosing() is called by OpenTK when the program closes; Calls an event called Unload() in Components</summary>
    /// <remarks> Sometimes Unload() may not call due to a force-close!</remarks>
    private static void OnUnload() {
        FirstUnload?.Invoke();
        
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(Event.Unload);
        Unload?.Invoke();
        
        FinalUnload?.Invoke();
    }





    /// <summary> An event that is raised before all update calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? FirstUpdate;
    
    public static event Action? Update;
    /// <summary> An event that is raised after all update calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? FinalUpdate;

    /// <summary> OnUpdateFrame() is called by OpenTK every frame before Drawing; Calls an event called Update() in Components</summary>
    /// <remarks> This is where you put your main logic!</remarks>
    private static void OnUpdate() {
        try {
            FirstUpdate?.Invoke();

            foreach (Scene scene in Osmium._scenes) if (scene.Enabled) scene.ChainEvent(Event.Update);
            Update?.Invoke();

            CoroutineRunner.Advance();

            FinalUpdate?.Invoke();
        }catch(Exception e) {
            TrySafeEscape(e);
        }
    }
    
    
    
    
    
    /// <summary> An event that is raised before all draw calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? FirstDraw;
    
    public static event Action? Draw;
    /// <summary> An event that is raised after all draw calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? FinalDraw;
    
    /// <summary> OnRenderFrame() is called by OpenTK every frame after Update; Calls an event called Draw() in Components</summary>
    /// <remarks> If you have Drawing logic you should put it in here!</remarks>
    private static void OnDraw() {
        try {
            FirstDraw?.Invoke();

            foreach (Scene scene in Osmium._scenes) if (scene.Enabled) scene.ChainEvent(Event.Draw);
            Draw?.Invoke();

            FinalDraw?.Invoke();
        }catch(Exception e) {
            TrySafeEscape(e);
        }
    }




    
    /// <summary> Attempts to safely close Osmium in the event of an Exception. So that
    /// finalizing statements may run, or so the entire Editor does not crash. </summary>
    /// <param name="e">The exception caught at runtime</param>
    private static void TrySafeEscape(Exception e) {
        Debug.Error("OSMIUM SAFE ESCAPE TRIGGERED!");
        Debug.Error("EXCEPTION THROWN : "  + e);

        if (IsVirtualized) VirtualClose();
        else Close();
    }
    
    
    
    
    
}