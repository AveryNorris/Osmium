namespace OsmiumEditor;

public static partial class DockerMap
{
    private static void Save() {
        string writeValue = FileWarningHeader + "\n^";

        foreach (SerializedScene mappedScene in Scenes)
        {
            writeValue += "\n" + mappedScene.scene.GetType().FullName + CompressDockerData(mappedScene);

            List<ComponentMapNode> TargetComponents = mappedScene.Children;

            foreach (ComponentMapNode component in TargetComponents)
            {
                writeValue += CompressComponentHierarchy(component);
            }
        }

        File.WriteAllText(Project.GetProjectSubdirectory(false, FilePath), writeValue);
    }

    public static string CompressComponentHierarchy(ComponentMapNode __component) {
        string writeValue = "\n ";

        for (int i = 0; i < __component.component.AllParents.Count; i++)
        {
            writeValue += ' ';
        }

        writeValue += __component.component.GetType().FullName + CompressDockerData(__component);

        foreach (ComponentMapNode mappedNode in __component.Children) {
            writeValue += CompressComponentHierarchy(mappedNode);
        }

        return writeValue;
    }
    

    public static string CompressDockerData(SerializedDocker serializedDocker) {
        string returnValue = string.Empty;
        
        foreach (SerializedVariable dockerDatum in serializedDocker.SerializedFields) {
            returnValue += "#" + dockerDatum.ToSaveData();
            //todo: constrain to save data
        }

        return returnValue;
    }
}