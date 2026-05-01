using System.Numerics;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OsmiumNucleus;
using OsmiumRadium;
using Vector4 = System.Numerics.Vector4;

namespace OsmiumRadium;

public static partial class Backend
{
    
    //todo: dispose

    internal static int ProgramHandle;
    
    internal static int VertexBufferHandle;
    internal static int IndexBufferHandle;

    internal static int VertexArrayHandle;

    internal static int XScalingFactor;

    public static bool ShouldUpdate = true;
    public static bool ShouldDraw = true;
    
    internal static readonly int[] Indices = [
        3, 2, 1,
        1, 0, 2
    ];

    internal static int DefaultTexture;
    
    internal static List<(int index, System.Numerics.Vector4 clippingRect)> ClippingRects = [];

    internal static HashSet<RadiumElement> RetainedElements = [];
    
    [MarkerAttributes.UnsafeInternal] internal static readonly SortedDictionary<int, List<ImGUI>> ZSortedElements = 
        new SortedDictionary<int, List<ImGUI>>(Comparer<int>.Create((a, b) => {
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

        using (StreamReader vertexReader = new(typeof(Backend).Assembly.GetManifestResourceStream("OsmiumRadium.Source.Backend.Vertex.glsl")!)) {
            GL.ShaderSource(VertexShader, vertexReader.ReadToEnd());
        }

        GL.CompileShader(VertexShader);
        
        GL.GetShaderInfoLog(VertexShader, out string VertexShaderInfo);
        if (VertexShaderInfo != string.Empty)
            throw new Exception(VertexShaderInfo);

        using (StreamReader fragmentReader = new(typeof(Backend).Assembly.GetManifestResourceStream("OsmiumRadium.Source.Backend.Fragment.glsl")!)) {
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
        Osmium.Context.UpdateFrame += Update;
        Osmium.Context.RenderFrame += Draw;

        Osmium.Context.TextInput += OnTextInput;
    }
    
    public static void Resize(ResizeEventArgs e) {
        GL.Viewport(0, 0, Osmium.Context.FramebufferSize.X, Osmium.Context.FramebufferSize.Y);
    }

    public static void Update(FrameEventArgs e) {

        if (ShouldUpdate)
        {
            foreach (RadiumElement element in RetainedElements)
            {
                element.Update();
            }
        }
    }

    public static int elementCount = 0;

    public static void Draw(FrameEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (ShouldDraw)
        {
            foreach (RadiumElement element in RetainedElements.ToList())
            {
                element.Draw();
            }
        }

        //resets clipping to normal; add default clipping value? todo:
        elementCount = 0;
        SetClipping(new Vector4(0,0,100,100));

        int currentClipping = 0;
        int index = 0;
        foreach(KeyValuePair<int, List<ImGUI>> ElementPriorityPairs in ZSortedElements) {
            foreach (ImGUI element in ElementPriorityPairs.Value) {
                if (currentClipping < ClippingRects.Count && ClippingRects[currentClipping].index == index) {
                    SetClipping(ClippingRects[currentClipping].clippingRect);
                    currentClipping++;
                }
                
                element.Draw();
                index++;
            }
        }
        
        ZSortedElements.Clear();
        ClippingRects.Clear();
        
        //placement doesnt make sense todo: also make sure that this becomes a method called clear state or something cool and sick and cool and sick ok bye thanks for talking make text boxes force ascii or something so that the font doesnt have a panic attack
        //todo: make text boxes work in whatever encoding strings support so that object names in scenes/components always match C#
        TextInput = "";
        
        Osmium.Context.SwapBuffers();
    }

    public static void SetClipping(System.Numerics.Vector4 __clippingRect) {
        int clippingRectUniform = GL.GetUniformLocation(ProgramHandle, "clippingRect");
        GL.Uniform4f(clippingRectUniform, __clippingRect.X, __clippingRect.Y, __clippingRect.Z, __clippingRect.W);
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
        
        //todo: cache uniforms
        int colorUniform = GL.GetUniformLocation(ProgramHandle, "color");
        GL.Uniform4f(colorUniform, color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
        
        
        //int colorUniform = GL.GetUniformLocation(ProgramHandle, "color");
        //GL.GetUniformf(ProgramHandle, colorUniform, [color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f]);
        
        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
    }
    
}