namespace OsmiumRadium;



public static partial class Backend
{
    
    
    public static int immediateElementCount = 0;
    
    
    /// <summary> A collection of all retained elements that are currently being handled </summary>
    private static readonly HashSet<RetainedElement> _retainedElements = [];
    /// <inheritdoc cref="_retainedElements"/>
    public static IReadOnlyCollection<RetainedElement> RetainedElements => _retainedElements;

    
    
    /// <summary> Creates a new instance of a retained element </summary>
    public static T Add<T>() where T : RetainedElement, new() {
        T newElement = new T();
        _retainedElements.Add(newElement);
        
        return newElement;
    }
    
    
    public static void Add(RetainedElement element) {
        _retainedElements.Add(element);
    }
    
    

    /// <summary> Removes the first found retained elements of a type </summary>
    public static void Remove<T>() where T : RetainedElement, new() => _retainedElements.Remove(_retainedElements.First(x => x.GetType() == typeof(T)));
    
    /// <summary> Removes an instance of a retained element </summary>
    public static void RemoveElement(RetainedElement retainedElement) => _retainedElements.Remove(retainedElement);
    

    public static void Clear() => _retainedElements.Clear();

    
}