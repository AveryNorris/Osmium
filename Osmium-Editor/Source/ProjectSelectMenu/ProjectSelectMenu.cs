    using Dear_ImGui_Sample.Backends;
    using ImGuiNET;
    using NativeFileDialogNET;
    using OpenTK.Mathematics;
    using OpenTK.Windowing.Common;
    using OsmiumNucleus;
    using Texture = Dear_ImGui_Sample.Backends.Types.Texture;
    using Vector2 = System.Numerics.Vector2;

    namespace OsmiumEditor;

    public static class ProjectMenu
    {

        private const ImGuiWindowFlags flags =
            ImGuiWindowFlags.NoResize |
            ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoTitleBar |
            ImGuiWindowFlags.NoScrollbar | 
            ImGuiWindowFlags.NoScrollWithMouse;

        private static readonly Texture OsmiumLogo;

        private static readonly ImFontPtr Jetbrains;
        
        static ProjectMenu() {
            
            //todo: TERRIBLE HORRIBLE AWFUL!
            Jetbrains = AssetLoader.Font("/home/averynorris/Osmium/Osmium-Editor/Assets/JetBrainsMonoNL-Regular.ttf", 55);
            OsmiumLogo = AssetLoader.Image("/home/averynorris/Osmium/Osmium-Editor/Assets/Osmium.png");
            
            ProjectMemory.RefreshProjectList();
        }

        public static void Initialize() {
            Bedrock.Draw += Draw;
        }

        private static void Draw() {

            //linux ;(

            if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS())
            {
                int sizeFactor = (int)(Bedrock.window.CurrentMonitor.ClientArea.Size.Y * .4f);
                
                Bedrock.window.ClientSize = new Vector2i(sizeFactor, sizeFactor);
                Bedrock.window.CenterWindow();
                Bedrock.window.WindowBorder = WindowBorder.Hidden;
            }

            ImGui.SetNextWindowPos(Vector2.Zero);
            ImGui.SetNextWindowSize(new Vector2(Osmium.Window.ClientSize.X, Osmium.Window.ClientSize.Y));
            ImGui.Begin("ProjectSelectMenu", flags);

            ImGui.Image(OsmiumLogo, new Vector2(55));

            ImGui.SameLine();

            ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(0 / 255f, 124 / 255f, 255 / 255f, 1));
            ImGui.PushFont(Jetbrains);
            ImGui.Text("Osmium");
            ImGui.PopFont();
            ImGui.PopStyleColor();

            ImGui.Separator();

            float margin = 10f;
            float gap = 2.5f;

            float availableWidth = Osmium.Window.ClientSize.X - (margin * 2) - gap;
            float buttonWidth = availableWidth / 2f;

            ImGui.SetCursorPosX(margin);

            if(ImGui.Button("Create", new Vector2(buttonWidth, 20)))
                CreateProjectPrompt();

            ImGui.SameLine(0, gap);

            if(ImGui.Button("Open", new Vector2(buttonWidth, 20)))
                OpenProjectPrompt();


            ImGui.BeginChild("ProjectList");
            ImGui.Dummy(new Vector2(0, 3));
            ImGui.Indent(10);
            ImGui.SetWindowFontScale(1.2f);
            for (int i = 0; i < ProjectMemory.Projects.Count; i++)
            {
                string name = Path.GetFileNameWithoutExtension(ProjectMemory.Projects[i]);

                Vector2 size = new Vector2(Osmium.Window.ClientSize.X, 55);

                Vector2 start = ImGui.GetCursorScreenPos();

                ImGui.InvisibleButton($"project_{i}", size);

                bool hovered = ImGui.IsItemHovered();
                bool clicked = ImGui.IsItemClicked();

                var draw = ImGui.GetWindowDrawList();

                if (hovered)
                {
                    draw.AddRectFilled(
                        start,
                        start + size,
                        ImGui.GetColorU32(ImGuiCol.HeaderHovered)
                    );
                }

                draw.AddText(start + new Vector2(10, 8), ImGui.GetColorU32(ImGuiCol.Text), name);
                
                ImGui.SetWindowFontScale(.85f);
                draw.AddText(start + new Vector2(10, 28), ImGui.GetColorU32(ImGuiCol.TextDisabled),
                    ProjectMemory.Projects[i]);
                ImGui.SetWindowFontScale(1.2f);

                if (clicked)
                {
                    Editor.OpenProject(ProjectMemory.Projects[i]);

                    Bedrock.Draw -= Draw;
                }
            }
            ImGui.EndChild();
            ImGui.End();
        }
        
        public static void CreateProjectPrompt() {
            using NativeFileDialog Dialog = new NativeFileDialog();
            Dialog.SaveFile().AddFilter("Osmium Projects", "osproj");
        
            DialogResult result = Dialog.Open(out string? output);
            if (result == DialogResult.Okay && output != null) {
                string projectName = Path.GetFileNameWithoutExtension(output);
                string projectFolderPath = Path.ChangeExtension(output, null);

                if (Directory.Exists(projectFolderPath)) {
                    Debug.Error("A folder with the given name already exists!", ["Path", "Name"], [projectFolderPath, projectName]);
                    return;
                }
                
                Directory.CreateDirectory(projectFolderPath);

                //todo: path.combine
                string projectFilePath = projectFolderPath + '/' + projectName + ".osproj";
                File.WriteAllText(projectFilePath, "Created using Osmium: V" + Osmium.Version);
                
                ProjectMemory.AppendProject(projectFilePath);
            }
        
            ProjectMemory.RefreshProjectList();
        }

        public static void OpenProjectPrompt() {
            using NativeFileDialog Dialog = new NativeFileDialog();
            Dialog.SelectFile().AddFilter("Osmium Projects", "osproj");
        
            DialogResult result = Dialog.Open(out string? output);
            if (result == DialogResult.Okay && output != null) {
                
                ProjectMemory.AppendProject(output);
            }
        
            ProjectMemory.RefreshProjectList();
        }
    }