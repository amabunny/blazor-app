using Microsoft.AspNetCore.Components;
namespace Star_Wars_DataBase.Data.CustomEvents.Fluent.TextSuggestions.OnTextSuggestionChanged;

public class TextSuggestionChangedArgs : EventArgs
{
    public required string Value { get; set; }
}

[EventHandler(
    "ontextsuggestionchanged", 
    typeof(TextSuggestionChangedArgs), 
    enableStopPropagation: true, 
    enablePreventDefault: true)]
public static class EventHandlers
{
    
}