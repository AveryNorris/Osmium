namespace OsmiumRadium;


public abstract class Element
{
    
    protected internal abstract void Draw();

    public void Introduce() {
        Backend.IMGUIElements.Add(this);
    }
    
}