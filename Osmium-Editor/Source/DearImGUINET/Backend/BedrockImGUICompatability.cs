using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OsmiumEditor.Source.DearImGUINET.Structure;
using OsmiumNucleus;
using Vector2 = System.Numerics.Vector2;

namespace Dear_ImGui_Sample.Backends;

public static class BedrockImGUICompatability
{

    public static Vector2 ScreenSize;
    
    public static void Incorporate() {
        Bedrock.Load += Load;
        Bedrock.Unload += Unload;
            
        Bedrock.DrawInitializer += DrawInitializer;
        Bedrock.DrawFinalizer += DrawFinalizer;

        Bedrock.window.Resize += (args => ScreenSize = new Vector2(Bedrock.window.ClientSize.X, Bedrock.window.ClientSize.Y));
    }

    private static void Load() {
        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);

        ImGui.CreateContext();
        ImGuiIOPtr io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        //io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

        ImGui.StyleColorsDark();

        ImGuiStylePtr style = ImGui.GetStyle();
        if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
        {
            style.WindowRounding = 0.0f;
            style.Colors[(int)ImGuiCol.WindowBg].W = 0.0f;
        }

        ImguiImplOpenTK4.Init(Bedrock.window);
        ImguiImplOpenGL3.Init();
    }

    private static void Unload() {
        ImguiImplOpenGL3.Shutdown();
        ImguiImplOpenTK4.Shutdown();
    }

    private static void DrawInitializer() {
        var io = ImGui.GetIO();
        
        ImguiImplOpenGL3.NewFrame();
        ImguiImplOpenTK4.NewFrame();
        ImGui.NewFrame();

        ImGui.DockSpaceOverViewport();

        //ImGui.ShowDemoWindow();
    }

    private static void DrawFinalizer() {
        ImGui.Render();
        ImguiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());

        if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.ViewportsEnable))
        {
            ImGui.UpdatePlatformWindows();
            ImGui.RenderPlatformWindowsDefault();
            Bedrock.window.Context.MakeCurrent();
        }
    }
}