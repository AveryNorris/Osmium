using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

namespace OsmiumNucleus;

public static class Bedrock
{
    
    
    
    public static readonly Window window = new Window();

    public static event Action Load;
    public static event Action Unload;

    public static event Action UpdateInitializer;
    public static event Action Update;
    public static event Action UpdateFinalizer;
    
    public static event Action DrawInitializer;
    public static event Action Draw;
    public static event Action DrawFinalizer;

    public static float DeltaTime;



    public static void Run() {
        window.Load += OnLoad;
        window.Unload += OnUnload;
        
        window.UpdateFrame += OnUpdate;

        window.RenderFrame += OnDraw;

        window.Resize += (ResizeEventArgs) => {
            GL.Viewport(0, 0, Bedrock.window.FramebufferSize.X, Bedrock.window.FramebufferSize.Y);
        };
        
        GL.ClearColor(0,0,0,0);
    }

    private static void OnLoad() {
        Load?.Invoke();
    }

    private static void OnUnload() {
        Unload?.Invoke();
    }

    private static void OnUpdate(FrameEventArgs e) {
        DeltaTime = (float) e.Time;
        
        UpdateInitializer?.Invoke();
        
        Update?.Invoke();
        
        UpdateFinalizer?.Invoke();
    }

    private static void OnDraw(FrameEventArgs e) {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        DeltaTime = (float) e.Time;
        
        DrawInitializer?.Invoke();
        
        Draw?.Invoke();
        
        DrawFinalizer?.Invoke();
        
        window.SwapBuffers();
    }
    
    
    //todo: Debug System??
    
    
}