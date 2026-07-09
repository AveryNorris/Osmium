using System.ComponentModel;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;



namespace OsmiumNucleus;


/// <summary> Bottom class of Osmium. Carries events from MonoGame into Scenes, and provides OpenTK context.</summary>
/// <author> Avery Norris </author>
public static partial class Osmium
{
    
    
    
    
    
    /// <summary> An event that is raised before all load calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? LoadInitializer;
    /// <summary> An event that is raised after all load calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? LoadFinalizer;

    
    
    /// <summary> OnLoad() is called by OpenTK when the program starts; Calls an event called Load() in Components</summary>
    /// <remarks> It is recommended to load content during Load()</remarks>
    private static void OnLoad() {
        try {
            LoadInitializer?.Invoke();

            foreach (Scene scene in Osmium._scenes) if (scene.Enabled) scene.ChainEvent(0);

            LoadFinalizer?.Invoke();
        }catch (Exception e) {
            TrySafeEscape(e);
        }
    }



    
    
    /// <summary> An event that is raised before all unload calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? UnloadInitializer;
    /// <summary> An event that is raised after all unload calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? UnloadFinalizer;
    
    /// <summary> OnClosing() is called by OpenTK when the program closes; Calls an event called Unload() in Components</summary>
    /// <remarks> Sometimes Unload() may not call due to a force-close!</remarks>
    private static void OnClosing(CancelEventArgs __args) {
        UnloadInitializer?.Invoke();
        
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(Event.Unload);
        
        UnloadFinalizer?.Invoke();
    }





    /// <summary> An event that is raised before all update calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? UpdateInitializer;
    /// <summary> An event that is raised after all update calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? UpdateFinalizer;

    /// <summary> OnUpdateFrame() is called by OpenTK every frame before Drawing; Calls an event called Update() in Components</summary>
    /// <remarks> This is where you put your main logic!</remarks>
    private static void OnUpdateFrame(FrameEventArgs __args) {
        try {
            UpdateInitializer?.Invoke();

            foreach (Scene scene in Osmium._scenes) if (scene.Enabled) scene.ChainEvent(Event.Update);

            CoroutineRunner.Advance();

            UpdateFinalizer?.Invoke();
        }catch(Exception e) {
            TrySafeEscape(e);
        }
    }
    
    
    
    
    
    /// <summary> An event that is raised before all draw calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? DrawInitializer;
    /// <summary> An event that is raised after all draw calls, this is meant to be used for libraries that require overhead </summary>
    public static event Action? DrawFinalizer;
    
    /// <summary> OnRenderFrame() is called by OpenTK every frame after Update; Calls an event called Draw() in Components</summary>
    /// <remarks> If you have Drawing logic you should put it in here!</remarks>
    private static void OnRenderFrame(FrameEventArgs __args) {
        try {
            DrawInitializer?.Invoke();

            foreach (Scene scene in Osmium._scenes) if (scene.Enabled) scene.ChainEvent(Event.Draw);

            DrawFinalizer?.Invoke();
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

        if (IsVirtualized) {
            VirtualClose();
        }else {
            Close();
        }
    }
    
    
    
    
    
}