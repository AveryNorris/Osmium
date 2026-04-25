using OpenTK.Windowing.Common;
using OsmiumNucleus;

namespace OsmiumEditor;

public static class UpdateTracker
{
    //todo: events that track change, public texture classes sign up to the events for their source path.



    private static FileSystemWatcher _watcher;

    internal static bool SurpressReload = false;
    
    public static void Initialize() {
        
        _watcher = new FileSystemWatcher {
            Path = Project.ProjectPath,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime,
        };
        
        _watcher.Changed += OnChanged;
        _watcher.Created += OnChanged;
        _watcher.Deleted += OnChanged;
        _watcher.Renamed += OnChanged;
    }

    public static void OnChanged(object sender, FileSystemEventArgs fileSystemEventArgs) {
        if(!SurpressReload) Context.Reload();
    }
    
    
}