namespace OsmiumRadium;

public class Element
{
    
    //todo: make Immedaite and Element an interface
    
    public Element() {
        Backend.immediateElementCount++;
    }
    
    
    
    /// <summary> Represents the ratio in a 16 / 9 screen </summary>
    protected const float r169 = 16f / 9f;
    
    
    
}