namespace OsmiumRadium;

public abstract class ImGUI
{

    public int z {
        get => _z;
        set {
            RemoveFromPrioritySortedElements();
            
            _z = value;
            
            AddToPrioritySortedElements();
        }
    } private int _z;

    protected ImGUI() {
        AddToPrioritySortedElements();
        Backend.elementCount++;
    }
    
    protected internal abstract void Draw();

    private void RemoveFromPrioritySortedElements() {
        Backend.ZSortedElements[_z].Remove(this);
        if (Backend.ZSortedElements[_z].Count == 0) Backend.ZSortedElements.Remove(_z);
    }
    
    private void AddToPrioritySortedElements() {
        if (!Backend.ZSortedElements.ContainsKey(_z)) Backend.ZSortedElements.Add(_z, []);
        Backend.ZSortedElements[_z].Add(this);
    }
    
}