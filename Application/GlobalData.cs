using System;
using Godot;

public partial class GlobalData : Node
{
    private static DayEventDisplay _selectedEventDisplay;
    private static EditPopup _editPopup;
    private static DateTime _selectedDateTime;

    public override void _Ready()
    {
        _editPopup = GetNode<EditPopup>("/root/Application/EditPopup");
    }

    public static void SetSelectedDateTime(DateTime date) => _selectedDateTime = date;
    public static DateTime GetSelectedDateTime() => _selectedDateTime;

    public void SetSelectedDisplay(DayEventDisplay display) => _selectedEventDisplay = display;

    public static DayEventDisplay GetSelectedDisplay() => _selectedEventDisplay;

    public static void OpenPopup()
    {
        DateEventData newData = new DateEventData("NEW EVENT")
        {
            StartDate = GetSelectedDateTime(),
            EndDate = GetSelectedDateTime(),
        };
        
        _editPopup.Initialize(newData, true);
    }
    
    public static void OpenPopup(DateEventData existingData)
    {
        _editPopup.Initialize(existingData);
    }
}
