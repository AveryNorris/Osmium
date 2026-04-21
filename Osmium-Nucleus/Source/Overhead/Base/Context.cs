using System.ComponentModel;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;



namespace OsmiumNucleus;


/// <summary> Bottom class of Osmium. Carries events from MonoGame into Scenes, and provides OpenTK context.</summary>
/// <author> Avery Norris </author>
public sealed class Context() : GameWindow(GameWindowSettings.Default, new NativeWindowSettings())
{
    
    

    /// <summary> OnLoad() is called by OpenTK when the program starts; Calls an event called Load() in Components</summary>
    /// <remarks> It is recommended to load content during Load()</remarks>
    protected override void OnLoad() {
        base.OnLoad(); 
        
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(0);
    }





    /// <summary> OnClosing() is called by OpenTK when the program closes; Calls an event called Unload() in Components</summary>
    /// <remarks> Sometimes Unload() may not call due to a force-close!</remarks>
    protected override void OnClosing(CancelEventArgs __args) {
        base.OnClosing(__args);
        
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(1);
    }


    
    

    /// <summary> OnUpdateFrame() is called by OpenTK every frame before Drawing; Calls an event called Update() in Components</summary>
    /// <remarks> This is where you put your main logic!</remarks>
    protected override void OnUpdateFrame(FrameEventArgs __args) {
        base.OnUpdateFrame(__args);
        
        Osmium.DeltaTime = __args.Time;
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(2);
    }
    
    
    
    
    
    /// <summary> OnRenderFrame() is called by OpenTK every frame after Update; Calls an event called Draw() in Components</summary>
    /// <remarks> If you have Drawing logic you should put it in here!</remarks>
    protected override void OnRenderFrame(FrameEventArgs __args) {
        base.OnRenderFrame(__args);
            
        Osmium.DeltaTime = __args.Time;
        foreach(Scene scene in Osmium._scenes) if(scene.Enabled) scene.ChainEvent(3);
    }
    
    
    
}