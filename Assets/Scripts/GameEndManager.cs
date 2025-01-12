using UnityEngine;

public class GameEndManager : MonoBehaviour, IManager
{
    private TurnManager turnManager;

    public void Initialize(GameController controller)
    {
        turnManager = controller.turnManager; // Retrieve dependency
        Debug.Log("GameEndManager initialized.");
    }

    public void CheckGameEnd()
    {
        Debug.Log("Checking game end conditions...");
        // Use turnManager or other dependencies here
    }
}
