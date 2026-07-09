namespace OsmiumEditor.Source.NewEditor.Actions;

public abstract class IRectifiableError
{

    public uint Duplicates { get; internal set; }
    
    public virtual bool CanStack(IRectifiableError __other) => Equals(__other);

    public abstract string Message { get; }
    
    public abstract ErrorRectifyAction Rectify();

}