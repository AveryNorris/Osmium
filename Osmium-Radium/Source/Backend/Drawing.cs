using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OsmiumNucleus;



namespace OsmiumRadium;



public static partial class Backend
{
    
    
    
    /// <summary> A static region that is permanent and readonly, all elements belong to it </summary>
    public static readonly StableRegion BaseRegion = new StableRegion();
    
    
    
    /// <summary> Determines if the backend should run update functions </summary>
    public static bool ShouldUpdate = true;
    /// <summary> Determines if the backend should run draw functions </summary>
    public static bool ShouldDraw = true;

    
    
    /// <summary> Updates all the retained elements </summary>
    public static void Update(FrameEventArgs e) {
        if (!ShouldUpdate) return;
            
        foreach (RetainedElement element in _retainedElements) element.Update();
    }
    
    
    
    /// <summary> Draws all the retained elements and immediate elements</summary>
    public static void Draw(FrameEventArgs e) {
        immediateElementCount = 0;
        
        if(!ShouldDraw) return;
        
        BaseRegion.Clear();
        
        
        foreach (RetainedElement element in _retainedElements.ToList()) {
            RegionState.ResetFocus();
                
            element.Draw();
        }

        BaseRegion.Draw();
    }
    
    
    
    /// <summary> Draws a single element from the given vertex data, the vertexes must be in 0-100 screen space
    /// and ordered like
    /// {
    /// max.x, max.y, 1, 1,
    /// min.x, max.y, 0, 1,
    /// max.x, min.y, 1, 0,
    /// min.x, min.y, 0, 0,
    /// }
    /// with two floats of position, and two floats of UV, repeated 4 times </summary>
    public static void DrawElement(Texture __texture, Color color, float __z, params float[] vertexData)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, __texture.Handle);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsage.DynamicDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferHandle);
        
        GL.BindVertexArray(_vertexArrayHandle);

        GL.UseProgram(_programHandle);
        
        GL.Uniform4f(_colorUniformHandle, color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
        GL.Uniform1f(_zUniformHandle, __z);
        
        GL.DrawElements(PrimitiveType.Triangles, _quadIndexLayout.Length, DrawElementsType.UnsignedInt, 0);
    }
    
    
    
    /// <summary> Draws a multiple elements from the given vertex data; each element vertex data must be in 0-100 screen space
    /// and ordered like
    /// {
    /// max.x, max.y, 1, 1,
    /// min.x, max.y, 0, 1,
    /// max.x, min.y, 1, 0,
    /// min.x, min.y, 0, 0,
    /// }
    /// with two floats of position, and two floats of UV, repeated 4 times,
    /// and you can repeat that structure to draw elements up until
    /// the MaxElementsPerDraw value </summary>
    public static void DrawElements(Texture __texture, int __count, Color color, float __z, params float[] vertexData)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, __texture.Handle);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsage.DynamicDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _largeIndexBuffer);
        
        GL.BindVertexArray(_vertexArrayHandle);

        GL.UseProgram(_programHandle);
        
        GL.Uniform4f(_colorUniformHandle, color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
        GL.Uniform1f(_zUniformHandle, __z);
        
        GL.DrawElements(PrimitiveType.Triangles, __count * 6, DrawElementsType.UnsignedInt, 0);
    }
    
}