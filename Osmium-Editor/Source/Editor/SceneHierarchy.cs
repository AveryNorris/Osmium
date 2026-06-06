using OsmiumNucleus;
using OsmiumRadium;

namespace OsmiumEditor;

public class SceneHierarchy() : EmbeddedWindow("Scenes")
{
    
    public static Scene? selectedScene;

    public const float sceneTextSize = 3;
    
    protected override void DrawEmbeddedWindow() {

        //todo: debug menu picks up embedded window, even though it is abstract, DO BETTER REFLECTION IN MANY SPOTS
        //todo: maybe make coords local to region?
        //todo: make sure mouse is in the window always
        //todo: borders on the headers of the embedded windows
        //todo: X's in the embedded windows
        
        int count = 0;
        foreach (Scene scene in Osmium.Scenes)
        {
            if (Button().Pos(Rect.pos.x, Rect.pos.y + 7.5f + sceneTextSize * count)
                .Size(Rect.size.x, sceneTextSize).Text(scene.Name).TextSize(sceneTextSize).Down())
            {
                selectedScene = scene;
            }
            count++;
        }
    }
}