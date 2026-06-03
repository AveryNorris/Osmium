namespace OsmiumNucleus;



/// <summary> An action taken during a coroutine. </summary>
public interface ICoroutineAction
{
    
    
    
    /// <summary> Processes the current Action in the coroutine </summary>
    /// <returns> Returns false if the coroutine should exit from this action. </returns>
    public bool Continue();
    
    
    
}