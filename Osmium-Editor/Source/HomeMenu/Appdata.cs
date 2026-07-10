using OsmiumNucleus;


namespace OsmiumEditor;


public static class Appdata
{


    public static readonly string Path;



    static Appdata() {
        string SystemAppdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        Path = SystemAppdataFolder + "/Osmium";

        if (!Directory.Exists(Path)) {
            Debug.Action("Failed to find an Appdata folder! Creating a new one...", ["Appdata Path"], [Path]);
            Directory.CreateDirectory(Path);
        } 
    }           



    public static void AssureExistence(string __path) {
        if (!File.Exists(__path)) {
            Debug.Action("Failed to find a config file! Creating a new one...", ["File Path"], [__path]);
            File.Create(__path).Close();
        }
    }
}