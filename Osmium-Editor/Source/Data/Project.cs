using OsmiumNucleus;

namespace OsmiumEditor;

public static class Project
{
    public static string ProjectPath;

    public static string RuntimeModulesPath => GetProjectSubdirectory("Modules", true);
    
    public static string SourcePath => GetProjectSubdirectory("Source", true);
        
        
    //todo: add global usage implciitly not in a file

    //todo: enforce that editor modules cannot define components, and runtime cannot use the editor etc
    public static string GetProjectSubdirectory(string subdirectoryPath, bool regenerate = false)
    {
        string path = Path.Combine(ProjectPath, subdirectoryPath);

            if (!Path.Exists(path))
            {
                Debug.Error("Requested Path Does Not Exist! ", ["Path"], [path]);

                if (regenerate)
                {
                    Debug.Error("Regenerating Project Subdirectory...", ["Path"], [path]);
                    
                    UpdateTracker.SurpressReload = true;
                    Directory.CreateDirectory(path);
                    UpdateTracker.SurpressReload = false;
                }
            }

        return path;
    }

    public static string GetProjectSubPath(string subpath, bool regenerate = false) {
        string path = Path.Combine(ProjectPath, subpath);

        if (!Path.Exists(path))
        {
            Debug.Error("Requested Path Does Not Exist! ", ["Path"], [path]);

            if (regenerate)
            {
                Debug.Action("Regenerating Project Subdirectory...", ["Path"], [path]);
                    
                UpdateTracker.SurpressReload = true;
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                
                if(!File.Exists(path))
                    File.Create(path);
                
                UpdateTracker.SurpressReload = false;
            }
        }

        return path;
    }
}