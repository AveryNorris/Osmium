using System.Reflection;
using OpenTK.Graphics.OpenGL.Compatibility;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Backend
{
    
    
    
    [GLHandle] private static readonly int _colorUniformHandle;
    [GLHandle] private static readonly int _clippingRectUniformHandle;
    [GLHandle] private static readonly int _programHandle;
    
    [GLHandle] private static readonly int _vertexBufferHandle;
    [GLHandle] private static readonly int _indexBufferHandle;
    [GLHandle] private static readonly int _vertexArrayHandle;
    
    [GLHandle] private static readonly int _largeIndexBuffer;
    
    public const int MaxElementsPerDraw = 10000;
    
    
    
    public static readonly Texture DefaultTexture;
    public static readonly Font DefaultFont;
    
    
    
    public static bool ShouldUpdate = true;
    public static bool ShouldClear = false;
    public static bool ShouldDraw = true;
    
    
    
    private static readonly HashSet<RetainedElement> _retainedElements = [];
    public static IReadOnlyCollection<RetainedElement> RetainedElements => _retainedElements;
    
    
    
    private static readonly int[] Indices = [
        3, 2, 1,
        1, 0, 2
    ];
    
    
    
    static Backend()
    {
        #region Shader
        _programHandle = GL.CreateProgram();

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        
        using (StreamReader vertexReader = new(typeof(Backend).Assembly.GetManifestResourceStream("OsmiumRadium.Source.Backend.Vertex.glsl")!))
            GL.ShaderSource(VertexShader, vertexReader.ReadToEnd());
        
        GL.CompileShader(VertexShader);

        using (StreamReader fragmentReader = new(typeof(Backend).Assembly.GetManifestResourceStream("OsmiumRadium.Source.Backend.Fragment.glsl")!)) 
            GL.ShaderSource(FragmentShader, fragmentReader.ReadToEnd());

        GL.CompileShader(FragmentShader);
        
        GL.UseProgram(_programHandle);
        
        GL.AttachShader(_programHandle, VertexShader);
        GL.AttachShader(_programHandle, FragmentShader);
        
        GL.LinkProgram(_programHandle);
        
        GL.DetachShader(_programHandle, VertexShader);
        GL.DeleteShader(VertexShader);

        GL.DetachShader(_programHandle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        
        #endregion
        
        #region Vertex Buffer
        
        _vertexBufferHandle = GL.GenBuffer();
        _indexBufferHandle = GL.GenBuffer();
        

        _vertexArrayHandle = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferHandle);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferHandle);
        
        GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(int), Indices, BufferUsage.DynamicDraw);
        
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        
        GL.UseProgram(_programHandle);
        
        GL.UseProgram(0);
        
        using (Stream stream = Assembly.GetAssembly(typeof(Backend))!.GetManifestResourceStream("OsmiumRadium.DefaultTexture.png")!) {
            DefaultTexture = Texture.LoadFromStream(stream);
        }
        
        using (Stream stream = Assembly.GetAssembly(typeof(Backend))!.GetManifestResourceStream("OsmiumRadium.proggyBitmapASCII.png")!) {
            DefaultFont = new Font(stream, 75, 19, [32,136]);
        }
        
        #endregion
        
        #region Index Buffer
        
        int[] indices = new int[MaxElementsPerDraw * 6];

        for (int i = 0; i < MaxElementsPerDraw; i++)
        {
            int v = i * 4;
            int idx = i * 6;

            indices[idx + 0] = v + 3;
            indices[idx + 1] = v + 2;
            indices[idx + 2] = v + 1;
            indices[idx + 3] = v + 1;
            indices[idx + 4] = v + 0;
            indices[idx + 5] = v + 2;
        }

        _largeIndexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _largeIndexBuffer);
        
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsage.StaticRead);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        
        #endregion
        
        #region Uniform
        
        _colorUniformHandle = GL.GetUniformLocation(_programHandle, "color");
        
        _clippingRectUniformHandle = GL.GetUniformLocation(_programHandle, "clippingRect");
        
        GL.Uniform1f(GL.GetUniformLocation(_programHandle, "tex"), 0);

        #endregion
        
        #region Events
        
        Osmium.Context.Resize += Resize;
        Osmium.Context.UpdateFrame += Update;
        Osmium.Context.RenderFrame += Draw;
        
        #endregion
    }
    
    
    
}