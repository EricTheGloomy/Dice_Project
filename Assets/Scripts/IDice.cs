public interface IDice
{
    int CurrentValue { get; }
    string Color { get; }
    void Roll();
}
