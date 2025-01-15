using UnityEngine;
using UnityEngine.EventSystems;

public class DiceSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject currentDice;
    
    public bool IsEmpty => currentDice == null;

    private void Awake()
    {
        // We could add any initialization here
    }

    public void OnDrop(PointerEventData eventData)
    {

        Debug.Log($"OnDrop called on slot: {gameObject.name}");
        
        // We can still see if the incoming object is a dice, but not re-parent it:
        var draggedDice = eventData.pointerDrag;
        if (draggedDice != null && draggedDice.GetComponent<DraggableDice>() != null)
        {
            Debug.Log($"Dice {draggedDice.name} dropped on {gameObject.name}, DraggableDice will handle re-parenting.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional: highlight the slot or do some feedback
        Debug.Log("Pointer entered the dice slot.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: remove highlight
        Debug.Log("Pointer exited the dice slot.");
    }

    public void AssignDice(GameObject dice)
    {
        currentDice = dice;
        currentDice.transform.SetParent(transform, false);

        var rectTransform = dice.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;
        
        Debug.Log($"Dice {dice.name} assigned to slot {gameObject.name}, new parent: {dice.transform.parent.name}");
    }

    public void ClearSlot()
    {
        if (currentDice != null)
        {
            currentDice = null;
            Debug.Log($"Slot {gameObject.name} cleared.");
        }
    }

    public GameObject GetCurrentDice()
    {
        return currentDice;
    }
}
