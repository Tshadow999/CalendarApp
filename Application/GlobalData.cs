using Godot;

public partial class GlobalData : Node
{
    private DayEventDisplay _selectedEventDisplay;

    public void SetSelectedDisplay(DayEventDisplay display) => _selectedEventDisplay = display;

    public DayEventDisplay GetSelectedDisplay() => _selectedEventDisplay;
}
