namespace OsmiumEditor.Source.NewEditor;

public static class ErrorCorrection
{
    public static HashSet<IPersistentCollectionError> ProcessErrors = [];
}

/// <summary> Represents an error which occurs during collection of persistent references </summary>
public interface IPersistentCollectionError
{
    public void Resolve();
}

public record MissingAssemblyError(string AssemblyName) : IPersistentCollectionError
{
    public void Resolve() {
        //prompt for renaming
    }
}

public record MissingTypeError(string AssemblyName, string TypeName) : IPersistentCollectionError
{
    public void Resolve() {
        //prompt for renaming
    }
}