using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text valueText;

    private ResourceSO resourceDefinition; // The data from ResourceSO

    public void Initialize(ResourceSO so, int startingValue)
    {
        resourceDefinition = so;
        if (iconImage != null && so.Icon != null)
        {
            iconImage.sprite = so.Icon;
        }
        UpdateValue(startingValue);
    }

    public void UpdateValue(int newValue)
    {
        if (valueText != null)
        {
            valueText.text = newValue.ToString();
        }
    }
}
