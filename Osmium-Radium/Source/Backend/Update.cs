using OpenTK.Windowing.Common;
using OsmiumNucleus;

namespace OsmiumRadium;

public static partial class Backend
{
    public static void Update(FrameEventArgs e) {
        
        if (ShouldUpdate)
        {
            foreach (RetainedElement element in _retainedElements)
            {
                element.Update();
            }
        }
    }
}