using Dear_ImGui_Sample.Backends;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class Editor
{
    public static int Main(string[] __args) {
        BedrockImGUICompatability.Incorporate();
        Osmium.EditorInitialize();

        Bedrock.Load += BedrockLoad;
        
        Bedrock.Run();
        
        Osmium.EditorRun();
        
        return 0;
    }

    private static void BedrockLoad() {
        EditorWindowHierarchy.Add<ProjectMenu>();
    }
}