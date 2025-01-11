using UnityEngine;

public class GameEndManager : MonoBehaviour, IManager
{
    public void Initialize()
    {
        Debug.Log("GameEndManager initialized.");
    }
    
    public void CheckGameEnd()
    {
        Debug.Log("Checking game end conditions...");
    }
}
