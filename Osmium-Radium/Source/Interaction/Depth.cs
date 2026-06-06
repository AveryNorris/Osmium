using OsmiumRadium;

public static class IDepthElementExtensions
{

    public const float DebuggingDepth = -1;
    public const float DraggingDepth = -.9999f;
    public const float Header = -.5f;
    public const float DefaultDepth = 0;
    public const float BackgroundDepth = 1;
    
    public static T Depth<T>(this T element, float depth) where T : ImmediateElement {
        element._depth = depth;
        element._overrideDepth = false;

        return element;
    }
}