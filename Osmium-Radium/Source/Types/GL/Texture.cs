using OpenTK.Graphics.OpenGL;
using OsmiumNucleus;
using StbImageSharp;


namespace OsmiumRadium;


/// <summary> Represents a texture loaded inside OpenGL. </summary>
[GLHandle]
public class Texture : IDisposable
{
    
    
    
    /// <summary> OpenGL handle of the _texture</summary>
    public readonly int Handle;
    
    
    /// <summary> Width of the image</summary>
    public readonly int Width;
    /// <summary> Height of the image</summary>
    public readonly int Height;


    /// <summary> Original path of the image! </summary>
    public readonly string Path;
    
    
    /// <summary> Creates a _texture from the contents of a given file </summary>
    /// <param name="__path"> Path of the file </param>
    private Texture(string __path)
    {

        if (__path == null) {
            Debug.Error("Texture path is null");
            return;
        }
        
        Path = __path;
        
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
    }

    private Texture(Stream __bitmapStream) {
        ImageResult image;
        
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

    public static Texture LoadFromFile(string __path) {
        return new Texture(__path);
    }

    public static Texture LoadFromStream(Stream __bitmapStream) {
        return new Texture(__bitmapStream);
    }

    public void Dispose() {
        GL.BindTexture(TextureTarget.Texture2d, Handle);
        
        GL.DeleteTexture(Handle);
        
        GL.BindTexture(TextureTarget.Texture2d, 0);
    }
}