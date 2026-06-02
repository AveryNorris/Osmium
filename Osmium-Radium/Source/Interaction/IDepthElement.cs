namespace OsmiumRadium;

public interface IDepthElement
{
    /// <summary> Z of the element </summary>
    public float _depth { get; set; }
}

public interface IDepthElement<out TSelf> where TSelf : ImmediateElement, IDepthElement<TSelf>, IDepthElement;


public static class IDepthElementExtensions
{
    public static T Depth<T>(this T element, float depth) where T : ImmediateElement, IDepthElement<T>, IDepthElement {
        element._depth = depth;

        return element;
    }
}