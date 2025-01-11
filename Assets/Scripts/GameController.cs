using UnityEngine;

public class GameController : MonoBehaviour
{
    public ResourceManager resourceManager;
    public DiceManager diceManager;
    public TurnManager turnManager;
    public GameEndManager gameEndManager;

    private void Awake()
    {
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        Debug.Log("Initializing managers...");
        resourceManager.InitializeResources();
        turnManager.resourceManager = resourceManager;
        turnManager.diceManager = diceManager;

        // Initialize managers
        diceManager.InitializeDicePool();
        Debug.Log("Managers initialized.");
    }

    private void Start()
    {
        Debug.Log("Starting the game...");
        turnManager.StartTurn();
    }
}
