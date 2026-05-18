
using OsmiumNucleus;
using OsmiumRadium;

namespace OsmiumRadium;

public static partial class Backend
{
    

    //todo: file customization for palettes?

    public static RetainedElement Add<T>() where T : RetainedElement, new() {
        T newElement = new T();
        _retainedElements.Add(newElement);
        
        return newElement;
    }

    public static void RemoveElement(RetainedElement retainedElement) => _retainedElements.Remove(retainedElement);
    
    public static void Remove<T>() where T : RetainedElement, new() {
        _retainedElements.Remove(_retainedElements.First(x => x.GetType() == typeof(T)));
    }
}