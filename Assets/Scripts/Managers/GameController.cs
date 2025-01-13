using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<IManager> managers = new List<IManager>();

    public ResourceManager resourceManager;
    public DiceManager diceManager;
    public TurnManager turnManager;
    public GameEndManager gameEndManager;

    private void Awake()
    {
        // Add all managers to the list
        managers.Add(resourceManager);
        managers.Add(diceManager);
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
