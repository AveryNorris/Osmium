using System.Numerics;
using OsmiumNucleus;
using OsmiumRadium;

namespace OsmiumRadium;

public static partial class Radium
{
    

    //todo: file customization for palettes?

    public static RadiumElement Add<T>() where T : RadiumElement, new() {
        T newElement = new T();
        _retainedElements.Add(newElement);
        
        return newElement;
    }

    public static void RemoveElement(RadiumElement __element) => _retainedElements.Remove(__element);
    
    public static void Remove<T>() where T : RadiumElement, new() {
        _retainedElements.Remove(_retainedElements.First(x => x.GetType() == typeof(T)));
    }
}