namespace OsmiumRadium;



/// <summary> Describes an element which can be a parent to other elements. It must hold a list of children elements </summary>
public abstract class Region : ImmediateElement
{
    
    
    
    /// <summary> All  the children elements belonging to the region, including further nested regions </summary>
    protected readonly List<ImmediateElement> _children = [];

    
    /// <summary> Adds an element to the region </summary>
    /// <param name="__element"> The element to add </param>
    public void Add(ImmediateElement __element) => _children.Add(__element);
    
    public bool Contains(ImmediateElement __element) => _children.Contains(__element);
    
    /// <summary> Returns the enumerator for all the elements at that time
    /// You can modify the regions list during this enumerator without errors </summary>
    public IEnumerator<ImmediateElement> GetEnumerator() => _children.ToList().GetEnumerator();
    
    
    
}