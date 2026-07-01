namespace OsmiumEditor.Source.NewEditor.Actions;

public interface IRectifiableError
{
    public bool Quiet { get; init; }

    public string ProgressMessage { get; init; }
    
    public void Communicate();
    
    public ErrorRectifyResult Rectify();
    
    public bool Stackable(IRectifiableError __other);
}