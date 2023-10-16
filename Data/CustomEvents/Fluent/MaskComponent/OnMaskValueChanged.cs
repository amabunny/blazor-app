using Microsoft.AspNetCore.Components;
namespace Star_Wars_DataBase.Data.CustomEvents.Fluent.MaskComponent.OnMaskValueChanged;

public class FluentMaskComponentChangedArgs : EventArgs
{
    public required string Value { get; set; }
}

[EventHandler(
    "onmaskvaluechanged", 
    typeof(FluentMaskComponentChangedArgs), 
    enableStopPropagation: true, 
    enablePreventDefault: true)]
public static class EventHandlers
{
    
}