using System.Drawing;
using System.Numerics;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Backend
{
    
    //todo: dispose

    internal static int ProgramHandle;
    
    internal static int VertexBufferHandle;
    internal static int IndexBufferHandle;

    internal static int VertexArrayHandle;
    
    internal static readonly int[] Indices = [
        3, 2, 1,
        1, 0, 2
    ];

    internal static int DefaultTexture;

    internal static List<GUIElement> Elements = [];
    
    [MarkerAttributes.UnsafeInternal] internal static readonly SortedDictionary<int, List<GUIElement>> ZSortedElements = 
        new SortedDictionary<int, List<GUIElement>>(Comparer<int>.Create((a, b) => {
            int result = a.CompareTo(b);
            return result; 
    }));
    

    static Backend()
    {

        ProgramHandle = GL.CreateProgram();

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        
        //create shaders
        
        //todo: rel paths or dll resource

        using (StreamReader vertexReader = new(typeof(Backend).Assembly.GetManifestResourceStream("OsmiumRadium.Source.Vertex.glsl")!)) {
            GL.ShaderSource(VertexShader, vertexReader.ReadToEnd());
        }

        GL.CompileShader(VertexShader);
        
        GL.GetShaderInfoLog(VertexShader, out string VertexShaderInfo);
        if (VertexShaderInfo != string.Empty)
            throw new Exception(VertexShaderInfo);

        using (StreamReader fragmentReader = new(typeof(Backend).Assembly.GetManifestResourceStream("OsmiumRadium.Source.Fragment.glsl")!)) {
            GL.ShaderSource(FragmentShader, fragmentReader.ReadToEnd());
        }

        GL.CompileShader(FragmentShader);
        
        GL.GetShaderInfoLog(FragmentShader, out string FragmentInfo);
        if(FragmentInfo != string.Empty)
            throw new Exception(FragmentInfo);
        
        
        //attach to program
        
        GL.UseProgram(ProgramHandle);
        
        GL.AttachShader(ProgramHandle, VertexShader);
        GL.AttachShader(ProgramHandle, FragmentShader);
        
        GL.LinkProgram(ProgramHandle);
        
        GL.DetachShader(ProgramHandle, VertexShader);
        GL.DeleteShader(VertexShader);

        GL.DetachShader(ProgramHandle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        
        GL.UseProgram(0);
        
        //vertex buffer
        
        VertexBufferHandle = GL.GenBuffer();
        IndexBufferHandle = GL.GenBuffer();
        

        VertexArrayHandle = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferHandle);
        
        GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(int), Indices, BufferUsage.DynamicDraw);
        
        //pos
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


        DefaultTexture = GL.GenTexture();
        
        GL.BindTexture(TextureTarget.Texture2d, DefaultTexture);

        byte[] data = [255, 255, 255, 255];

        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, 1, 1, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.BindTexture(TextureTarget.Texture2d, 0);
        
        GL.UseProgram(ProgramHandle);
        
        int texUniform = GL.GetUniformLocation(ProgramHandle, "tex");
        GL.Uniform1f(texUniform, 0);
        
        GL.UseProgram(0);
        
        //events

        Osmium.Context.Resize += Resize;
        Osmium.Context.RenderFrame += Draw;
    }
    
    public static void Resize(ResizeEventArgs e) {
        GL.Viewport(0, 0, Osmium.Context.FramebufferSize.X, Osmium.Context.FramebufferSize.Y);
    }

    public static void Draw(FrameEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        foreach(KeyValuePair<int, List<GUIElement>> ElementPriorityPairs in ZSortedElements) {
            foreach (GUIElement element in ElementPriorityPairs.Value) {
                element.Draw();
            }
        }
        
        ZSortedElements.Clear();
        
        Osmium.Context.SwapBuffers();
    }

    
    /// <summary>
    /// Draws a single square from the given vertex data! Vertices must be signed by
    /// +, +
    /// -, +
    /// +, -
    /// -, -
    /// </summary>
    /// <param name="vertexData"></param>
    public static void DrawElement(int __texture, Color color, params float[] vertexData)
    {
        GL.BindTexture(TextureTarget.Texture2d, __texture);
        GL.ActiveTexture(TextureUnit.Texture0);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsage.DynamicDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferHandle);
        
        GL.BindVertexArray(VertexArrayHandle);

        GL.UseProgram(ProgramHandle);
        
        int colorUniform = GL.GetUniformLocation(ProgramHandle, "color");
        GL.Uniform4f(colorUniform, color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        
        
        //int colorUniform = GL.GetUniformLocation(ProgramHandle, "color");
        //GL.GetUniformf(ProgramHandle, colorUniform, [color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f]);
        
        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
    }
    
}