using Microsoft.AspNetCore.Components;

namespace Star_Wars_DataBase.Shared;

public interface IDispatchable
{
    public void Dispatch();

    public event Action Subscriptions;
}

public class BlazorState<T> : IDispatchable
{
    private T _value;

    public T Value
    {
        get => _value;
        set => Change(value);
    }

    public BlazorState(T value)
    {
        _value = value;
    }

    public void Change(T value)
    {
        _value = value;
        Dispatch();
    }

    public void Change(Func<T, T> fn)
    {
        Change(fn(_value));
    }

    public void Dispatch()
    {
        Subscriptions?.Invoke();
    }

    public event Action? Subscriptions;
}

public class BlazorStateAdapter : IDispatchable, IDisposable
{
    private static readonly string LogPrefix = "[BlazorStateAdapter]:";
    private bool _debug;

    private readonly List<Action> _updaters = new();

    public BlazorStateAdapter ConnectContainerState<TValue>(
        BlazorState<TValue> containerValue,
        Func<TValue, TValue, bool>? shouldRenderFn = null)
    {
        if (Subscriptions == null && _debug)
        {
            Console.Error.WriteLine($"{LogPrefix} adapter has no active subscriptions. " +
                                    $"You might forgot to call base class initializer. Place the base.OnInitialized(); " +
                                    $"right before connecting to container property.");
        }

        var cachedValue = containerValue.Value;

        void Updater()
        {
            if (shouldRenderFn != null)
            {
                var needUpdate = shouldRenderFn(containerValue.Value, cachedValue);

                if (needUpdate)
                {
                    if (_debug)
                        Console.WriteLine($"{LogPrefix} state updater decided to update {containerValue.Value}");
                    Dispatch();
                }
            }
            else
            {
                if (_debug)
                    Console.WriteLine(
                        $"{LogPrefix} state updater decided to update [{containerValue.GetType()}] {containerValue.Value}");
                Dispatch();
            }
        }

        containerValue.Subscriptions += Updater;
        _updaters.Add(Updater);

        if (_debug)
            Console.WriteLine($"{LogPrefix} state connected to adapter");

        Dispatch();

        return this;
    }

    public BlazorStateAdapter Debug()
    {
        _debug = true;
        return this;
    }

    public void Dispatch()
    {
        if (_debug)
            Console.WriteLine($"{LogPrefix} dispatching render from adapter");
        Subscriptions?.Invoke();
    }

    public void Dispose()
    {
        if (_debug)
            Console.WriteLine($"{LogPrefix} Disposing Adapter");

        _updaters.ForEach(updater => { Subscriptions -= updater; });
    }

    public event Action? Subscriptions;
}

public class BlazorStateComponent : ComponentBase, IDisposable
{
    protected readonly BlazorStateAdapter StateAdapter = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        StateAdapter.Subscriptions += StateHasChanged;
    }

    public void Dispose()
    {
        StateAdapter.Subscriptions -= StateHasChanged;
        StateAdapter.Dispose();
    }
}