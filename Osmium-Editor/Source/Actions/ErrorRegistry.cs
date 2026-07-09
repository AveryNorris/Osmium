using OsmiumNucleus;

namespace OsmiumEditor.Source.NewEditor.Actions;

public static class ErrorRegistry
{
    private static readonly List<IRectifiableError> _errors = [];

    public static IRectifiableError? Current { get; private set; }
    
    public static IReadOnlyList<IRectifiableError> Errors => _errors;

    static ErrorRegistry() {
        Bedrock.Update += Update;
    }

    public static void Add(IRectifiableError __newError) {
        if (__newError == null) {
            Debug.Error("Cannot rectify a null error!");
            return;
        }
        
        foreach (IRectifiableError error in _errors.ToArray()) {
            if (error.CanStack(__newError)) {
                error.Duplicates++;
                return;
            }
        }
        
        _errors.Add(__newError);
    }

    public static void Update() {
        if (Current != null && _errors.Count > 0) Current = _errors[0];
        
        if (Current != null) TryRectifyCurrent();
    }

    public static void TryRectifyCurrent() {
        ErrorRectifyAction action = Current!.Rectify();

        if (action == ErrorRectifyAction.Continue) {
            Current = null;
        }

        if (action == ErrorRectifyAction.Panic) {
            Debug.Error("ERROR PANIC! CLOSING OSMIUM");
            Osmium.Close();
        }
    }
}