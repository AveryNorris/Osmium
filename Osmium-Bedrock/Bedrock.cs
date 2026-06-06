using OpenTK.Graphics.OpenGL4;

namespace OsmiumNucleus;

public static class Bedrock
{
    
    
    
    public static readonly Window window = new Window();

    public static event Action Load;
    public static event Action Unload;
    
    public static event Action Update;
    public static event Action Draw;

    public static float DeltaTime;



    public static void Run() {
        window.Load += () => Load?.Invoke();
        window.Unload += () => Load?.Invoke();
        
        window.UpdateFrame += (FrameEventArgs) => {
            DeltaTime = (float) FrameEventArgs.Time;
            Update?.Invoke();
        };
        
        window.RenderFrame += (FrameEventArgs) => {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            DeltaTime = (float) FrameEventArgs.Time;
            Draw?.Invoke();
            window.SwapBuffers();
        };

        //todo FIRST SUSPICION, move to render frame if it doesnt work!
        window.Resize += (ResizeEventArgs) => {
            GL.Viewport(0, 0, Bedrock.window.FramebufferSize.X, Bedrock.window.FramebufferSize.Y);
        };
    }
    
    
    
    
}