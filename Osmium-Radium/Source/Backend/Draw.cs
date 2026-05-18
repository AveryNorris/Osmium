
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Backend
{
    
    
    
    public static StableRegion BaseRegion = new StableRegion();
    public static bool DrawingImmediate { get; private set; }
    
    public static int elementCount = 0;

    
    
    
    
    /// <summary> Draws all the retained elements and immediate elements</summary>
    /// <param name="e"></param>
    public static void Draw(FrameEventArgs e) {
        UploadClippingUniform(new Bounds(size: new Vector2(100,100)));
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        if (ShouldDraw) {
            foreach (RetainedElement element in _retainedElements.ToList()) {
                RegionState.ResetFocus();
                
                element.Draw();
            }
        }

        DrawingImmediate = true;

        //resets clipping to normal; add default clipping value? todo:
        elementCount = 0;
        UploadClippingUniform(new Bounds(size: new Vector2(100,100)));

        BaseRegion.Draw();

        BaseRegion.Clear();
        
        //placement doesnt make sense todo: also make sure that this becomes a method called clear state or something cool and sick and cool and sick ok bye thanks for talking make _text boxes force ascii or something so that the _font doesnt have a panic attack
        //todo: make _text boxes work in whatever encoding strings support so that object names in scenes/components always match C#
        
        Osmium.Context.SwapBuffers();

        DrawingImmediate = false;
    }
    
    
    
    
    
    public static void DrawElement(Texture __texture, Color color, params float[] vertexData)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, __texture.Handle);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsage.DynamicDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferHandle);
        
        GL.BindVertexArray(_vertexArrayHandle);

        GL.UseProgram(_programHandle);
        
        //todo: cache uniforms
        GL.Uniform4f(_colorUniformHandle, color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
        
        
        //int colorUniform = GL.GetUniformLocation(ProgramHandle, "_color");
        //GL.GetUniformf(ProgramHandle, colorUniform, [_color.R / 255f, _color.G / 255f, _color.B / 255f, _color.A / 255f]);
        
        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
    }
    
    
    
    
    
    public static void DrawElements(Texture __texture, int __count, Color color, params float[] vertexData)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, __texture.Handle);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsage.DynamicDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _largeIndexBuffer);
        
        GL.BindVertexArray(_vertexArrayHandle);

        GL.UseProgram(_programHandle);
        
        //todo: cache uniforms
        GL.Uniform4f(_colorUniformHandle, color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
        
        
        //int colorUniform = GL.GetUniformLocation(ProgramHandle, "_color");
        //GL.GetUniformf(ProgramHandle, colorUniform, [_color.R / 255f, _color.G / 255f, _color.B / 255f, _color.A / 255f]);
        
        GL.DrawElements(PrimitiveType.Triangles, __count * 6, DrawElementsType.UnsignedInt, 0);
    }
}