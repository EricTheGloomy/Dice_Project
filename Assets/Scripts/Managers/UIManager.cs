using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour, IManager
{
    public GameObject dicePrefab;
    public GameObject diceUIContainerPrefab;
    public Canvas canvas;

    public Button rollDiceButton;
    public Button endTurnButton;

    private Transform diceUIContainer;

    public void Initialize(GameController controller)
    {
        GameObject containerInstance = Instantiate(diceUIContainerPrefab, canvas.transform);
        diceUIContainer = containerInstance.transform;

        rollDiceButton.onClick.AddListener(controller.dicePoolManager.RollAllDice);
        endTurnButton.onClick.AddListener(controller.turnManager.EndTurn);

        Debug.Log("UIManager initialized.");
    }

    // Create and link a Dice UI element to a Dice
    public GameObject CreateDiceUI(Dice dice)
    {
        GameObject diceUI = Instantiate(dicePrefab, diceUIContainer);

        var diceUIComponent = diceUI.GetComponent<DiceUI>();
        if (diceUIComponent != null && diceUIComponent.faceImage != null)
        {
            diceUIComponent.faceImage.color = dice.LogicalColor.DisplayColor;
            diceUIComponent.faceImage.sprite = dice.CurrentSprite;
        }
        else
        {
            Debug.LogWarning("DiceUI component or faceImage not set up correctly in prefab.");
        }

        return diceUI;
    }

    // Update the Dice UI to reflect the current state of the Dice
    public void UpdateDiceUI(GameObject diceUIObject, Sprite faceSprite)
    {
        if (diceUIObject != null)
        {
            var diceUIComponent = diceUIObject.GetComponent<DiceUI>();
            if (diceUIComponent != null && diceUIComponent.faceImage != null)
            {
                diceUIComponent.faceImage.sprite = faceSprite;
            }
            else
            {
                Debug.LogWarning("DiceUI component or faceImage not set up correctly in prefab.");
            }
        }
        else
        {
            Debug.LogWarning("DiceUIObject is null.");
        }
    }

    // Clear and destroy UI for temporary dice
    public void ClearDiceUI(List<Dice> tempDicePool)
    {
        foreach (var dice in tempDicePool)
        {
            if (dice.UIContainerObject != null)
            {
                Destroy(dice.UIContainerObject);
                dice.UIContainerObject = null;
            }
        }
        Debug.Log("Temporary dice UI cleared.");
    }
}
