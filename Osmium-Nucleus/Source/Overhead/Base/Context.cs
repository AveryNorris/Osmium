using System.ComponentModel;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;



namespace OsmiumNucleus;


/// <summary> Bottom class of Osmium. Carries events from MonoGame into Scenes, and provides OpenTK context.</summary>
/// <author> Avery Norris </author>
public sealed class Context() : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() {DepthBits = 24})
{
    
    
    
    
    
    /// <summary> An event that is raised before all load calls, this is meant to be used for libraries that require overhead </summary>
    public event Action? LoadInitializer;
    /// <summary> An event that is raised after all load calls, this is meant to be used for libraries that require overhead </summary>
    public event Action? LoadFinalizer;

    
    
    /// <summary> OnLoad() is called by OpenTK when the program starts; Calls an event called Load() in Components</summary>
    /// <remarks> It is recommended to load content during Load()</remarks>
    protected override void OnLoad() {
        LoadInitializer?.Invoke();
        
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(0);
        
        LoadFinalizer?.Invoke();
        
        base.OnLoad(); 
    }



    
    
    /// <summary> An event that is raised before all unload calls, this is meant to be used for libraries that require overhead </summary>
    public event Action? UnloadInitializer;
    /// <summary> An event that is raised after all unload calls, this is meant to be used for libraries that require overhead </summary>
    public event Action? UnloadFinalizer;
    
    /// <summary> OnClosing() is called by OpenTK when the program closes; Calls an event called Unload() in Components</summary>
    /// <remarks> Sometimes Unload() may not call due to a force-close!</remarks>
    protected override void OnClosing(CancelEventArgs __args) {
        UnloadInitializer?.Invoke();
        
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(1);
        
        UnloadFinalizer?.Invoke();
        
        base.OnClosing(__args);
    }





    /// <summary> An event that is raised before all update calls, this is meant to be used for libraries that require overhead </summary>
    public event Action? UpdateInitializer;
    /// <summary> An event that is raised after all update calls, this is meant to be used for libraries that require overhead </summary>
    public event Action? UpdateFinalizer;

    /// <summary> OnUpdateFrame() is called by OpenTK every frame before Drawing; Calls an event called Update() in Components</summary>
    /// <remarks> This is where you put your main logic!</remarks>
    protected override void OnUpdateFrame(FrameEventArgs __args) {
        UpdateInitializer?.Invoke();
        
        Osmium.DeltaTime = (float) __args.Time;
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(2);
        
        CoroutineRunner.Advance();
        
        UpdateFinalizer?.Invoke();
        
        base.OnUpdateFrame(__args);
    }
    
    
    
    
    
    /// <summary> An event that is raised before all draw calls, this is meant to be used for libraries that require overhead </summary>
    public event Action? DrawInitializer;
    /// <summary> An event that is raised after all draw calls, this is meant to be used for libraries that require overhead </summary>
    public event Action? DrawFinalizer;
    
    /// <summary> OnRenderFrame() is called by OpenTK every frame after Update; Calls an event called Draw() in Components</summary>
    /// <remarks> If you have Drawing logic you should put it in here!</remarks>
    protected override void OnRenderFrame(FrameEventArgs __args) {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        DrawInitializer?.Invoke();
        
        Osmium.DeltaTime = (float) __args.Time;
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(3);
        
        DrawFinalizer?.Invoke();
        
        base.OnRenderFrame(__args);
        
        SwapBuffers();
    }
    
    
    
    
    
}