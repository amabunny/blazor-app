namespace Star_Wars_DataBase.Shared.Containers;

public class CounterWithDifferentScopesContainer
{
    public readonly BlazorState<int> CounterValue = new(0);

    public void Increment()
    {
        CounterValue.Change(currentCount => ++currentCount);
    }

    public void Decrement()
    {
        CounterValue.Change(currentCount => --currentCount);
    }
}