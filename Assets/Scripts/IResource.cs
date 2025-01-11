public interface IResource
{
    string Name { get; }
    int CurrentValue { get; set; }
    int MaxValue { get; set; }
    void Add(int amount);
    void Deduct(int amount);
    bool IsDepleted();
}