using System.Diagnostics;
using Debug = OsmiumNucleus.Debug;

namespace OsmiumEditor;

public static partial class Editor
{
    public static void Compile() {
        File.WriteAllText(Path.Combine(Project.ProjectPath, ".compilationTopLevel.cs"), TopLevelNugget);
        Process? process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "build project.csproj",
            WorkingDirectory = Project.ProjectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        });

        if (process == null)
        {
            Debug.Error("Failed to create build process.");
        }
        
        string output = process!.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Console.WriteLine(output);
        Console.Error.WriteLine(errors);

        Console.WriteLine($"Exit code: {process.ExitCode}");
        
        File.Delete(Path.Combine(Project.ProjectPath, ".compilationTopLevel.cs"));
    }
}