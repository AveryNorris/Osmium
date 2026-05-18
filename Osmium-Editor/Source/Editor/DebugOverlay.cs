using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;
using OsmiumRadium;

namespace OsmiumEditor;

public class DebugOverlay : RetainedElement
{
    private bool DebugMenu = false;
    
    
    private float frameratesum = 0;
    private int frameratecount = 0;

    private List<float> FrameRates = [];
    
    protected override void Update() {
        if (Input.GetKeyDown(Keys.RightShift)) {
            DebugMenu = !DebugMenu;
        }
    }

    protected override void Draw() {
        if (DebugMenu) {
            FrameRates.Add(1f / Osmium.DeltaTime);
            if (FrameRates.Count > 1000) {
                FrameRates.RemoveAt(0);
            }
            
            frameratesum += Osmium.DeltaTime;
            frameratecount++;
            
            TextBox().Text("AFPS : " + (int)(1f / (frameratesum / frameratecount))).TextSize(5).Spacing(.45f, 1).Size(100).TextColor(Color.FromRgb(255,255,0));
            
            TextBox().Text("IFPS : " + (1f / Osmium.DeltaTime)).Pos(26,0).Size(100).TextSize(5).Spacing(.45f, 1).TextColor(Color.FromRgb(0,255,255));
            TextBox().Text(" > " + FrameRates.Min()).Pos(26,5).TextSize(5).Size(100).Spacing(.45f, 1).TextColor(Color.FromRgb(0,255,255));
            TextBox().Text(" < " + FrameRates.Max()).Pos(26,10).TextSize(5).Spacing(.45f, 1).Size(100).TextColor(Color.FromRgb(0,255,255));

            
            TextBox().Text("Elements : " + OsmiumRadium.Backend.RetainedElements.Count).TextSize(5).Pos(62,0).Size(100).Spacing(.45f, 1).TextColor(Color.FromRgb(255,0,255));
            
            TextBox().Text("Screen Size : (" + OsmiumRadium.Backend.WindowWidth + ',' + OsmiumRadium.Backend.WindowHeight + ") : " + OsmiumRadium.Backend.WindowWidthHeightRatio).TextSize(5).Pos(0,95).Size(100).Spacing(.45f, 1).TextAnchor(TextAnchor.TopLeft).TextColor(Color.FromRgb(0,255,0));
        }
        else
        {
            frameratesum = 0;
            frameratecount = 0;
            FrameRates.Clear();
        }
    }
}