using Dear_ImGui_Sample.Backends;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumNucleus;

namespace OsmiumEditor;

public static class Startup
{
    public static int Main(string[] __args) {
        BedrockImGUICompatability.Incorporate();
        Osmium.EditorInitialize();

        Bedrock.Load += Load;
        
        Bedrock.Run();
        
        Osmium.EditorRun();
        
        return 0;
    }

    public static void Load() {
        EditorWindowHierarchy.Add<ProjectMenu>();
    }
}