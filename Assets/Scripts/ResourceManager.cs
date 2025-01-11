using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IManager
{
    public List<ResourceSO> resourceConfigurations; // Assign via Inspector
    private Dictionary<ResourceSO, Resource> resources;

    public void Initialize()
    {
        InitializeResources();
        Debug.Log("ResourceManager initialized.");
    }

    public void InitializeResources()
    {
        resources = new Dictionary<ResourceSO, Resource>();

        foreach (var config in resourceConfigurations)
        {
            resources[config] = new Resource(config.Name, config.StartingValue, config.MaxValue);
            Debug.Log($"Initialized resource: {config.Name} with {config.StartingValue}/{config.MaxValue}");
        }
    }

    public void AddResource(ResourceSO resourceSO, int amount)
    {
        if (resources.ContainsKey(resourceSO))
        {
            resources[resourceSO].Add(amount);
            Debug.Log($"Added {amount} to {resourceSO.Name}. Current: {resources[resourceSO].CurrentValue}/{resources[resourceSO].MaxValue}");
        }
    }

    public void DeductResource(ResourceSO resourceSO, int amount)
    {
        if (resources.ContainsKey(resourceSO))
        {
            resources[resourceSO].Deduct(amount);
            Debug.Log($"Deducted {amount} from {resourceSO.Name}. Current: {resources[resourceSO].CurrentValue}/{resources[resourceSO].MaxValue}");
        }
    }

    public int GetResourceValue(ResourceSO resourceSO)
    {
        return resources.ContainsKey(resourceSO) ? resources[resourceSO].CurrentValue : 0;
    }
}
