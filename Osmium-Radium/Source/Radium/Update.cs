using OpenTK.Windowing.Common;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Radium
{
    public static void Update(FrameEventArgs e) {

        //move this somewhere where it makes sense
        MousePos = new System.Numerics.Vector2(100 * Osmium.Context.MousePosition.X / Osmium.Context.ClientSize.X, 100 * Osmium.Context.MousePosition.Y / Osmium.Context.ClientSize.Y);
        
        if (ShouldUpdate)
        {
            foreach (RadiumElement element in _retainedElements)
            {
                element.Update();
            }
        }
    }
}