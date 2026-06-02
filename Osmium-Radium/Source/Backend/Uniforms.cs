
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Backend
{
    public static Bounds Clipping;
    
    /// <summary> Since radium exists at retained draw time and realtime draw time, it is important
    /// to have an additional field which shows what the clipping should be at that time</summary>
    public static Bounds PredictedClipping;
    
    public static float WindowWidth;

    public static float WindowHeight;
    
    public static float WindowWidthHeightRatio { get; private set; }
    
    
    /// <summary> Uploads a new clipping rect to the graphics card  </summary>
    /// <param name="__clippingRect"></param>
    public static void UploadClippingUniform(Bounds __clippingRect) {
        Clipping = __clippingRect;
        GL.UseProgram(_programHandle);
        GL.Uniform4f(_clippingRectUniformHandle, __clippingRect.min.x, __clippingRect.min.y, __clippingRect.max.x, __clippingRect.max.y);
        GL.UseProgram(0);
    }

    /// <summary> Uploads a new clipping uniform as a subset of the current uniform and the given rectangle </summary>
    /// <param name="__subclip"> The rectangle to clip </param>
    public static void UploadSubclippingUniform(Bounds __subclip) {
        Bounds clippingRect = new Bounds(
            min: new Vector2(MathF.Max(Clipping.min.x, __subclip.min.x), MathF.Max(Clipping.min.y, __subclip.min.y)),
            max : new Vector2(MathF.Min(Clipping.max.x,  __subclip.max.x), MathF.Min(Clipping.max.y, __subclip.max.y))
        );
        
        UploadClippingUniform(clippingRect);
    }
    
    public static void UploadSubclippingUniform(Bounds __parentclip, Bounds __subclip) {
        Bounds clippingRect = new Bounds(
            min: new Vector2(MathF.Max(__parentclip.min.x, __subclip.min.x), MathF.Max(__parentclip.min.y, __subclip.min.y)),
            max : new Vector2(MathF.Min(__parentclip.max.x,  __subclip.max.x), MathF.Min(__parentclip.max.y, __subclip.max.y))
        );
        
        UploadClippingUniform(clippingRect);
    }
    
    public static void Resize(ResizeEventArgs e) {
        GL.Viewport(0, 0, Osmium.Context.FramebufferSize.X, Osmium.Context.FramebufferSize.Y);
        WindowWidth = Osmium.Context.Size.X;
        WindowHeight = Osmium.Context.Size.Y;
        WindowWidthHeightRatio = WindowWidth / WindowHeight;
    }
    
    
    
}