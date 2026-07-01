namespace OsmiumEditor.Source.NewEditor.Serialization;

public interface Data
{
    public string ToSaveData();
    
    public static abstract Data FromSaveData(string saveData);
}