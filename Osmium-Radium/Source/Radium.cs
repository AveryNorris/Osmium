using System.Numerics;
using OsmiumNucleus;
using OsmiumRadium;


public static class Radium
{
    

    //todo: file customization for palettes?

    public static RadiumElement Add<T>() where T : RadiumElement, new() {
        T newElement = new T();
        Backend.RetainedElements.Add(newElement);
        
        return newElement;
    }

    public static void RemoveElement(RadiumElement __element) => Backend.RetainedElements.Remove(__element);
    
    public static void Remove<T>() where T : RadiumElement, new() {
        Backend.RetainedElements.Remove(Backend.RetainedElements.First(x => x.GetType() == typeof(T)));
    }
}