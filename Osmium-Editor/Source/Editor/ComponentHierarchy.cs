using OsmiumNucleus;
using OsmiumRadium;

namespace OsmiumEditor;

public class ComponentHierarchy() : EmbeddedWindow("Hierarchy")
{
    
    public static Component? selectedComponent;
    
    public const float componentTextSize = 3;
    
    protected override void DrawEmbeddedWindow() {

        if (SceneHierarchy.selectedScene == null) return;
        
        //todo: maybe make coords local to region?
        //todO: embedded window border broken
        int count = 0;
        foreach (Component component in SceneHierarchy.selectedScene)
        {
            if (Button().Pos(Rect.pos.x, Rect.pos.y + 7.5f + componentTextSize * count)
                .Size(Rect.size.x, componentTextSize).TextSize(componentTextSize).Text(component.GetType().Name).Down())
            {
                selectedComponent = component;
            }
            count++;
        }
    }
}