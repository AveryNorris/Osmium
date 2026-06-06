using OsmiumNucleus;
using OsmiumRadium;


namespace OsmiumEditor;

public class DebugConsole() : EmbeddedWindow("Console")
{

    private float scroll = 0;
    
    protected override void DrawEmbeddedWindow() {
        //todo: use something like rope for the debug console?
        //todo: make a list element that iterates and places a type of object at each iteration of something
        //todo: delete debug.output
        //todo: make it so the windows stack in a queue or something, and the mouse dragging one puts it on top?

        const float xOffset = 1;
        const float yOffset = 6;

        const float messageSize = 3;
        
        Group(Debug.Stack.ToArray(), (count, message) => {
            TextBox().Text(message.Key.Message).Pos(Rect.min.x + xOffset, Rect.min.y + yOffset + count * messageSize).Size(100).TextSize(2f).TextColor(Palette.Text).Depth(.5f);
        }).Rect(Rect).Scrollable(ref scroll, Rect.min.y + yOffset + Debug.Stack.Count * messageSize);
    }
}