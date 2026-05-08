namespace OsmiumRadium;


public interface ITexturedElement<out TSelf> where TSelf : IElement, ITexturedElement<TSelf>
{
    /// <summary> Texture of the element </summary>
    public Texture _texture { get; set; }
}


public static class ITexturedElementExtensions
{
    public static T Texture<T>(this T element, Texture texture) where T : IElement, ITexturedElement<T> {
        element._texture = texture;

        return element;
    }
}