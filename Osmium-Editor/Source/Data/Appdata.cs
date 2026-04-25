using OsmiumNucleus;


namespace OsmiumEditor;


public static class Appdata
{


    public static readonly string Path;



    static Appdata() {
        string SystemAppdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        Path = SystemAppdataFolder + "/Osmium";

        if (!Directory.Exists(Path)) {
            Debug.LogAction("Failed to find an Appdata folder! Creating a new one...", ["Appdata Path"], [Path]);
            Directory.CreateDirectory(Path);
        } 
    }           


    
    public static string GetFileAt(string __localPath) {
        string realPath = Path + '/' + __localPath;
        
        AssureExistence(__localPath);
        
        return File.ReadAllText(realPath);
    }



    public static void SaveText(string __localPath, string __text) {
        string realPath = Path + '/' + __localPath;
        
        AssureExistence(__localPath);
        
        File.WriteAllText(realPath, __text);
    }



    public static void AppendText(string __localPath, string __text) {
        string realPath = Path + '/' + __localPath;
        
        AssureExistence(__localPath);
        
        File.AppendAllText(realPath, __text);
    }



    public static void AssureExistence(string __path) {
        
        //todo: make resistant to false middle directories
        if (!File.Exists(__path)) {
            Debug.LogAction("Failed to find a config file! Creating a new one...", ["File Path"], [__path]);
            File.Create(__path).Close();
        }
    }
}