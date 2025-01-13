using UnityEngine;

public class Resource
{
    public string Name { get; private set; }
    public int CurrentValue { get; private set; }
    public int MaxValue { get; private set; }

    public Resource(string name, int startingValue, int maxValue)
    {
        Name = name;
        CurrentValue = startingValue;
        MaxValue = maxValue;
    }

    public void Add(int amount) => CurrentValue = Mathf.Min(CurrentValue + amount, MaxValue);
    public void Deduct(int amount) => CurrentValue = Mathf.Max(CurrentValue - amount, 0);
}
