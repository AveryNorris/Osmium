namespace Dear_ImGui_Sample.Backends.Types;

public record Texture(IntPtr Handle, int Width, int Height)
{

    public static implicit operator IntPtr(Texture texture) => texture.Handle;
}