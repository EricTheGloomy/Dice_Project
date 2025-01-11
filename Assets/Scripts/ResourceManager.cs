using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public void InitializeResources()
    {
        Debug.Log("Initializing resources...");
    }

    public void AddResource(string name, int amount)
    {
        Debug.Log($"Adding {amount} to resource: {name}");
    }
}