using Dear_ImGui_Sample.Backends.Types;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Dear_ImGui_Sample.Backends;

public static class AssetLoader
{
    public static ImFontPtr Font(string path, float size) {
        return ImGui.GetIO().Fonts.AddFontFromFileTTF(path, size);
    }

    static AssetLoader() {
        ImGui.GetIO().Fonts.AddFontDefault();
    }
    
    //todo: dont use embedded resources? research a better way to manage assets
    public static Texture Image(string path) {
        int texture = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, texture);
        
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Linear
        );

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear
        );
        
        ImageResult image = ImageResult.FromStream(File.OpenRead(path));
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

        return new Texture(texture, image.Width, image.Height);
    }
}