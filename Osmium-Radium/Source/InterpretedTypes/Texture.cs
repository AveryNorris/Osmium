using System.Numerics;
using OpenTK.Graphics.OpenGL;
using OsmiumNucleus;
using StbImageSharp;


namespace OsmiumRadium;


/// <summary> Represents an OpenGL _texture which can be drawn; textures loaded from the same path twice will be recognized, and will not create duplicates; in other words
/// it is ok to make a new _texture of a file every frame; it will not reload the file</summary>
/// <remarks> You can call Dispose() to free up the GL resources, but this will cause the image to have to be reloaded the next time you use it so call this carefully. </remarks>
public class Texture : IDisposable
{
    
    
    
    /// <summary> OpenGL handle of the _texture</summary>
    public int Handle;
    
    
    /// <summary> Width of the image</summary>
    public int Width;
    /// <summary> Height of the image</summary>
    public int Height;


    /// <summary> Original path of the image! </summary>
    public readonly string Path;
    
    
    //easy _texture syntax
    public static implicit operator int(Texture __texture) => __texture.Handle;
    
    /// <summary> Contains stored memory of textures so that they don't need to be reloaded if they are inside openGL</summary>
    private static Dictionary<string, (int Handle, int Width, int Height)> TextureMemory = [];

    
    
    /// <summary> Creates a _texture from the contents of a given file </summary>
    /// <param name="__path"> Path of the file </param>
    public Texture(string __path)
    {

        if (__path == null) {
            Debug.Error("Texture path is null");
            return;
        }
        
        Path = __path;
        
        if (TextureMemory.TryGetValue(__path, out (int Handle, int Width, int Height) TextureInformation)) {
            Handle = TextureInformation.Handle;
            Width = TextureInformation.Width;
            Height = TextureInformation.Height;
            return;
        }
        
        ImageResult image;
        
        //opens the _font, todo: dont hardcode
        using (Stream fileStream = File.OpenRead(__path))
        {
            image = ImageResult.FromStream(fileStream, ColorComponents.RedGreenBlueAlpha);
        }

        Width = image.Width;
        Height = image.Height;
        
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2d, Handle);
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.BindTexture(TextureTarget.Texture2d, 0);
        
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        
        
        TextureMemory.Add(__path, (Handle, Width, Height));
    }

    public Texture(Stream __bitmapStream) {
        //todo: bye bye _texture memory
        
        ImageResult image;
        
        //opens the _font, todo: dont hardcode
        using (Stream fileStream = __bitmapStream)
        {
            image = ImageResult.FromStream(fileStream, ColorComponents.RedGreenBlueAlpha);
        }

        Width = image.Width;
        Height = image.Height;
        
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2d, Handle);
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.BindTexture(TextureTarget.Texture2d, 0);
        
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    /// <summary> Frees up GL resources </summary>
    public void Dispose() {
        GL.BindTexture(TextureTarget.Texture2d, Handle);
        
        TextureMemory.Remove(Path);
        GL.DeleteTexture(Handle);
        
        GL.BindTexture(TextureTarget.Texture2d, 0);
    }
}