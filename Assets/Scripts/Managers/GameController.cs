using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<IManager> managers = new List<IManager>();

    public UIManager uiManager;
    public ResourceManager resourceManager;
    public DiceManager diceManager;
    public DicePoolManager dicePoolManager;
    public TurnManager turnManager;
    public GameEndManager gameEndManager;

    private void Awake()
    {
        // Add all managers to the list
        managers.Add(uiManager);
        managers.Add(resourceManager);
        managers.Add(diceManager);
        managers.Add(dicePoolManager);
        managers.Add(turnManager);
        managers.Add(gameEndManager);

        InitializeManagers();
    }

    private void InitializeManagers()
    {
        foreach (var manager in managers)
        {
            manager.Initialize(this); // Pass GameController as the dependency container
        }

        Debug.Log("All managers initialized with dependencies.");
    }

    private void Start()
    {
        Debug.Log("Game starting...");
        turnManager.StartTurn();
    }
}
