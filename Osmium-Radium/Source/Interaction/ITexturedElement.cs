namespace OsmiumRadium;

public interface ITexturedElement
{
    /// <summary> Texture of the element </summary>
    public Texture _texture { get; set; }
}

public interface ITexturedElement<out TSelf> where TSelf : ImmediateElement, ITexturedElement<TSelf>, ITexturedElement;


public static class ITexturedElementExtensions
{
    public static T Texture<T>(this T element, Texture texture) where T : ImmediateElement, ITexturedElement<T>, ITexturedElement {
        element._texture = texture;

        return element;
    }
}