namespace OsmiumNucleus;



/// <summary> Manages and runs coroutines </summary>
internal static class CoroutineRunner
{

    
    /// <summary> A list of all the routines currently being ran </summary>
    private static readonly List<IEnumerator<ICoroutineAction>> CurrentRoutines = [];
    
    
    
    /// <summary> Begins a coroutine, which will advance asynchronously every frame. </summary>
    /// <param name= "__coroutine"></param>
    public static void Start(IEnumerator<ICoroutineAction> __coroutine) {
        CurrentRoutines.Add(__coroutine);
    }



    /// <summary> Advances and calls all coroutines that frame </summary>
    public static void Advance() {
        for (int i = 0; i < CurrentRoutines.Count; i++) {
            ICoroutineAction? current = CurrentRoutines[i].Current;
            
            if(current == null) { CurrentRoutines[i].MoveNext(); continue; }
            if (current.Continue()) continue;
            
            if (!CurrentRoutines[i].MoveNext()) CurrentRoutines.RemoveAt(i);
        }
    }
    
    
    
}