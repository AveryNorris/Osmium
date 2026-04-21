using System.Drawing;
using OsmiumNucleus;

namespace OsmiumRadium;

public class Palette
{
    private readonly Color[] colors = [];
    
    public Color this[uint index]
    {
        get {
            if (index >= colors.Length) {
                Debug.LogError("Pallete does not have a definition for the given color index!");
                return Color.Magenta;
            }
    
            return colors[index];
        } set {
            if (index >= colors.Length)
                Debug.LogError("Pallete does not have a definition for the given color index!");
            
            colors[index] = value;
        }
    }



    public Palette(params Color[] __colors)
    {
        colors = __colors;
    }
}