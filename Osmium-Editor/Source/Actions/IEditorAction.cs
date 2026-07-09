namespace OsmiumEditor.Source.NewEditor.Actions;

public interface IEditorAction
{
    public void Undo();

    public void Redo();

    public bool Group(IEditorAction __previousAction);
}