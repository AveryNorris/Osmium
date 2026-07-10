using System.Reflection;
using OsmiumNucleus;

namespace OsmiumEditor;

public static partial class DockerMap
{
    public static void Load() {
        string path = Project.GetProjectSubdirectory(false, FilePath);

        if (!File.Exists(path)) return;
        
        string[] saveData = File.ReadAllText(path).Split("^");

        if (saveData.Length == 0)
        {
            Debug.Log("Invalid or NonPresent save data!");
            return;
        }

        string text = saveData[^1];

        List<SerializedDocker> GeneratedTree = [];

        foreach (string member in text.Split("\n"))
        {
            if (member == string.Empty)
                continue;
            
            int depth = 0;
            
            for (int i = 0; i < member.Length; i++)
            {
                if (member[i] == ' ')
                    depth++;
                else break;
            }

            foreach (SerializedDocker node in GeneratedTree.ToArray())
            {
                if (GeneratedTree.IndexOf(node) >= depth)
                {
                    GeneratedTree.Remove(node);
                }
            }

            SerializedDocker newSerializedDocker;
            
            string[] data = member.Substring(depth).Split("#");
            if (data.Length == 0)
            {
                Debug.Error("Corrupt save data!");
                continue;
            }
            
            //todo: add try catches to events for all plugins, and the game running, and turn off the map when the game is running.
            
            Type? foundType = null;

            foreach (Assembly assembly in Context.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.FullName == data[0])
                    {
                        foundType = type;
                    }
                }
            }
            
            if (foundType == null)
            {
                Debug.Error("Failure! Failed to find a defined type", ["TypeName"], [data[0]]);
                continue;
            }
            
            //todo: totally assumes that the type is a scene or component based on depth, double check that! and make sure there is no tricky buisness with
            //todo: custom component or scenes that inherit one or the other
            if (depth == 0)
            {
                //todo: make a class that verifies that save data is not corrupted, no matching names etc

                //todo: can things inherit scenes avoid a constructor with just string?
                OsmiumNucleus.Scene scene = Activator.CreateInstance(foundType, "MAP_GENERATED_SCENE") as OsmiumNucleus.Scene;

                Osmium.AddScene(scene);
                    
                newSerializedDocker = FindMappedScene(scene);
            }
            else
            {
                Component component = Activator.CreateInstance(foundType) as Component;
                
                GeneratedTree[^1].docker.Add(component);
                
                newSerializedDocker = FindMappedComponent(component);
            }

            if (newSerializedDocker == null)
                return;

            for (int i = 1; i < data.Length; i++)
            {
                SerializedVariable? newField = SerializedVariable.FromSaveData(foundType, data[i]);

                if (newField != null && newField.IsValid)
                {
                    newSerializedDocker.SerializedFields.Add(newField);

                    newField.Load(newSerializedDocker);
                }
            }
            
            GeneratedTree.Add(newSerializedDocker);
        }
    }
}