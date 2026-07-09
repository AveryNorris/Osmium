namespace OsmiumEditor.Source;

public static class Editor
{

    public static event Action OnSave;

    public static void Save() {
        OnSave?.Invoke();
    }
}