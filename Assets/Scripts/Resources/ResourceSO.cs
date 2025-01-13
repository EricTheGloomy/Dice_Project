using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Resources/Resource")]
public class ResourceSO : ScriptableObject
{
    public string Name;
    public Sprite Icon; 
    public int StartingValue;
    public int MaxValue;
}
