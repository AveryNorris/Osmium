using System.Numerics;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Radium
{
    public static Vector4 Clipping;
    

    private static List<Vector4> Subclips = [];
    
    
    public static float WindowWidth;

    public static float WindowHeight;
    
    public static float WindowWidthHeightRatio { get; private set; }
    
    
    
    /// <summary> Immediately  </summary>
    /// <param name="__clippingRect"></param>
    public static void UploadClippingUniform(Vector4 __clippingRect) {
        Clipping = __clippingRect;
        GL.UseProgram(_programHandle);
        GL.Uniform4f(_clippingRectUniformHandle, __clippingRect.X, __clippingRect.Y, __clippingRect.Z, __clippingRect.W);
        GL.UseProgram(0);
    }

    public static void UploadSubclippingUniform(Vector4 __subclip) {
        Vector4 clippingRect = new Vector4(
            MathF.Max(Clipping.X, __subclip.X), MathF.Max(Clipping.Y, __subclip.Y),
            MathF.Min(Clipping.Z,  __subclip.Z), MathF.Min(Clipping.W, __subclip.W)
        );
        
        Subclips.Add(Clipping);
        UploadClippingUniform(clippingRect);
    }
    
    
    public static void RevertSubclippingBounds() {
        if (Subclips.Count > 0)
        {
            UploadClippingUniform(Subclips[^1]);
            Subclips.RemoveAt(Subclips.Count - 1);
        }
        else
        {
            Debug.Error("Failed to revert subclipping bounds");
        }
    }
    
    
    
    public static void Resize(ResizeEventArgs e) {
        GL.Viewport(0, 0, Osmium.Context.FramebufferSize.X, Osmium.Context.FramebufferSize.Y);
        WindowWidth = Osmium.Context.Size.X;
        WindowHeight = Osmium.Context.Size.Y;
        WindowWidthHeightRatio = WindowWidth / WindowHeight;
    }
}