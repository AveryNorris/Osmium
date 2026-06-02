using System.Reflection;
using OpenTK.Graphics.OpenGL.Compatibility;
using OsmiumNucleus;

namespace OsmiumRadium;



/// <summary> The backend of Radium! Manages rendering through draw and update calls,
/// and orders retained and immediate UI events at the appropriate times </summary>
public static partial class Backend
{
 
    
    
    /// <summary> Points to the color uniform in the fragment shader </summary>
    [GLHandle] private static readonly int _colorUniformHandle;
    /// <summary> Points to the clipping rect uniform in the fragment shader </summary>
    [GLHandle] private static readonly int _clippingRectUniformHandle;
    /// <summary> Points to the z uniform in the vertex shader </summary>
    [GLHandle] private static readonly int _zUniformHandle;
    /// <summary> Points to the shader program used to draw everything </summary>
    [GLHandle] private static readonly int _programHandle;
    
    
    
    /// <summary> Points to the vertex buffer which stores temporary vertex data</summary>
    [GLHandle] private static readonly int _vertexBufferHandle;
    /// <summary> Points to the index buffer which stores temporary index data</summary>
    [GLHandle] private static readonly int _indexBufferHandle;
    /// <summary> Points to the vertex array buffer used to draw </summary>
    [GLHandle] private static readonly int _vertexArrayHandle;
    
    
    
    /// <summary> Points to a large static index buffer, used to batch large draw calls, size of the buffer is controlled
    /// by the max elements per draw parameter </summary>
    [GLHandle] private static readonly int _largeIndexBuffer;
    /// <summary> The max amount of elements that can be used in a DrawElements call,
    /// this is an arbitrary value, but increasing this more uses up VRAM so use it wisely. </summary>
    public const int MaxElementsPerDraw = 4096;
    
    
    
    /// <summary> A default white 1x1 texture </summary>
    public static readonly Texture DefaultTexture;
    /// <summary> The default proggyclean font</summary>
    public static readonly Font DefaultFont;
    
    
    
    /// <summary> The correct index buffer for a single quad </summary>
    private static readonly int[] _quadIndexLayout = [
        3, 2, 1,
        1, 0, 2
    ];
    
    
    //todo : layers
    
    /// <summary> Initializes the backend completely </summary>
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
        
        GL.BufferData(BufferTarget.ElementArrayBuffer, _quadIndexLayout.Length * sizeof(int), _quadIndexLayout, BufferUsage.DynamicDraw);
        
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
            DefaultFont = Font.FromBitmapStream(stream, 75, 19, [32,136]);
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
        
        _zUniformHandle = GL.GetUniformLocation(_programHandle, "z");
        
        GL.Uniform1f(GL.GetUniformLocation(_programHandle, "tex"), 0);

        #endregion
        
        #region Events
        
        Osmium.Context.Resize += Resize;
        Osmium.Context.UpdateFrame += Update;
        Osmium.Context.RenderFrame += Draw;
        
        #endregion
        
        GL.Enable(EnableCap.DepthTest);
        GL.ClearDepth(1);
        GL.DepthFunc(DepthFunction.Lequal);
        
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcColor, BlendingFactor.OneMinusSrcAlpha);
        
        GL.DepthMask(true);
        
        //todo: add a debug that uses object and turns it into a string and a few ones that have int and whatnot
        //and they use preferrable tostring params, add some tostring params to my pretty types as well, like component
        Debug.Log(GL.GetInteger(GetPName.DepthBits).ToString());
    }
    
    
    
}