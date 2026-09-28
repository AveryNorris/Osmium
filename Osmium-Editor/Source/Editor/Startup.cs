using Dear_ImGui_Sample.Backends;
using OsmiumBedrock;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    public static int Main(string[] __args) {
        BedrockImGUICompatability.Incorporate();
        Osmium.EditorInitialize();

        Bedrock.Load += BedrockLoad;
        
        Bedrock.Run();
        
        Debug.RegisterCategory("EDITOR");
        Osmium.EditorRun();
        
        return 0;
    }

    private static void BedrockLoad() {
        ProjectMenu.Initialize();
    }
}