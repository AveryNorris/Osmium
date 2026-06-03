using OsmiumNucleus;
using OsmiumRadium;


namespace OsmiumEditor;


public static class ProjectMemory
{

    //todo: ban the use of app domain?
    public static string ProjectRefPath = Appdata.Path + "/projectRef";


    public static bool ValidOsmiumProject(string __path) {
        //todo: check syntax
        return File.Exists(__path) && Path.GetExtension(__path) == ".osproj";
    }
    
    public static List<string> Projects;

    public static void RefreshProjectList() {
        Appdata.AssureExistence(ProjectRefPath);
        List<string> refreshedProjectLinks = [];
        List<DateTime> refreshedProjectTimes = [];

        string projectData = "";
        bool update = false;
        foreach (string projectPointer in File.ReadAllLines(ProjectRefPath)) {
            string[] contents = projectPointer.Split("|");
            
            if (contents.Length < 2 || !DateTime.TryParse(contents[0], out DateTime time) || !ValidOsmiumProject(contents[1])) {
                update = true; Debug.Error("Failed to parse an OsPtr! It will be ignored.", ["Path"], [projectData]); continue;
            }

            //time
            //contents[1]

            if (refreshedProjectLinks.Contains(projectPointer)) {
                update = true; 
                //error
                Debug.Error("Project ref has duplicate projects!", ["Path"], [projectData]); continue;
            }
                
            refreshedProjectLinks.Add(contents[1]);
            refreshedProjectTimes.Add(time);
            projectData += projectPointer + "\n";
        }
        
        //update project data
        if (update) {
            File.WriteAllText(ProjectRefPath, projectData);
        }

        Projects = refreshedProjectLinks.Zip(refreshedProjectTimes, (a, b) => new { Link = a, Time = b}).OrderByDescending(x => x.Time).Select(x => x.Link).ToList();
    }



    public static void AppendProject(string __projectPath) {
        Appdata.AssureExistence(ProjectRefPath);
        RefreshProjectList();

        if (!ValidOsmiumProject(__projectPath)) return;
        if (Projects.Contains(__projectPath)) {
            //error
            return;
        }
        
        File.AppendAllText(ProjectRefPath, DateTime.Now.ToString("G") + "|" + __projectPath + '\n');
        
        RefreshProjectList();
    }



    public static void ForgetProject(string __projectPath) {
        Appdata.AssureExistence(ProjectRefPath);
        RefreshProjectList();

        List<string> newFileData = [];
        
        foreach (string line in File.ReadAllLines(ProjectRefPath)) {
            string[] contents = line.Split("|");
            if (contents.Length >= 2 && contents[1] != __projectPath) {
                newFileData.Add(line);
            }
        }
        
        File.WriteAllLines(ProjectRefPath, newFileData);
        RefreshProjectList();
    }



    public static void RefreshProjectTime(string __projectPath) {
        ForgetProject(__projectPath);
        AppendProject(__projectPath);
    }



    public static string GetProjectName(string __projectPath)
    {
        return Path.GetFileNameWithoutExtension(__projectPath);
    }
    
}