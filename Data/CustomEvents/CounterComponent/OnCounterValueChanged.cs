using Microsoft.AspNetCore.Components;
namespace Star_Wars_DataBase.Data.CustomEvents.CounterComponent.OnCounterValueChanged;

public class CounterComponentChangedArgs : EventArgs
{
    public bool? Increment { get; set; }
    public bool? Decrement { get; set; }
}

[EventHandler(
    "oncountervaluechanged", 
    typeof(CounterComponentChangedArgs), 
    enableStopPropagation: true, 
    enablePreventDefault: true)]
public static class EventHandlers
{
    
}
