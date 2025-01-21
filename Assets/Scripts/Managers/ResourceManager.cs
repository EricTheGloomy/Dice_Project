using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IManager
{
    public List<ResourceSO> resourceConfigurations;
    private Dictionary<ResourceSO, Resource> resources;

    [Header("Resource UI")]
    public Transform resourceUIContainer; // Where resource UIs appear
    public GameObject resourceUIPrefab;   // The prefab with ResourceUI script

    // Keep track of each ResourceSO's UI to update it
    private Dictionary<ResourceSO, ResourceUI> resourceUIMap = new Dictionary<ResourceSO, ResourceUI>();

    public void Initialize(GameController controller)
    {
        InitializeResources();
        Debug.Log("ResourceManager initialized.");
        CreateResourceUIs();
    }

    private void InitializeResources()
    {
        resources = new Dictionary<ResourceSO, Resource>();
        foreach (var config in resourceConfigurations)
        {
            resources[config] = new Resource(config.Name, config.StartingValue, config.MaxValue);
            Debug.Log($"Initialized resource: {config.Name} with {config.StartingValue}/{config.MaxValue}");
        }
    }

    private void CreateResourceUIs()
    {
        if (resourceUIContainer == null || resourceUIPrefab == null)
        {
            Debug.LogWarning("ResourceManager: UI container or prefab not assigned.");
            return;
        }

        // For each ResourceSO, spawn a UI element
        foreach (var config in resourceConfigurations)
        {
            GameObject uiObj = Instantiate(resourceUIPrefab, resourceUIContainer);
            ResourceUI uiComp = uiObj.GetComponent<ResourceUI>();
            int startingValue = resources[config].CurrentValue;
            uiComp.Initialize(config, startingValue);

            resourceUIMap[config] = uiComp;
        }
    }

    public void AddResource(ResourceSO resourceSO, int amount)
    {
        if (resources.ContainsKey(resourceSO))
        {
            resources[resourceSO].Add(amount);
            Debug.Log($"Added {amount} to {resourceSO.Name}. Current: {resources[resourceSO].CurrentValue}/{resources[resourceSO].MaxValue}");

            // Update UI
            if (resourceUIMap.TryGetValue(resourceSO, out ResourceUI uiComp))
            {
                uiComp.UpdateValue(resources[resourceSO].CurrentValue);
            }
        }
    }

    public void DeductResource(ResourceSO resourceSO, int amount)
    {
        if (resources.ContainsKey(resourceSO))
        {
            resources[resourceSO].Deduct(amount);
            Debug.Log($"Deducted {amount} from {resourceSO.Name}. Current: {resources[resourceSO].CurrentValue}/{resources[resourceSO].MaxValue}");

            // Update UI
            if (resourceUIMap.TryGetValue(resourceSO, out ResourceUI uiComp))
            {
                uiComp.UpdateValue(resources[resourceSO].CurrentValue);
            }
        }
    }

    public int GetResourceValue(ResourceSO resourceSO)
    {
        return resources.ContainsKey(resourceSO) ? resources[resourceSO].CurrentValue : 0;
    }
}
