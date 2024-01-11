using System;
using Godot;

public partial class EventButtons : HBoxContainer
{
	private DateEventData _eventData;
	private DateEventData _editedData;

	private GlobalData _globalData;

	public override void _Ready()
	{
		base._Ready();
		_globalData = GetNode<GlobalData>("/root/GlobalData");
	}

	private void OnAddButtonPressed_Signal()
	{
		GlobalData.OpenPopup();
	}

	private void OnEditButtonPressed_Signal()
	{
		// Do this when done
		// ICalFileReader.EditDateEvent(_eventData, _editedData);
	}

	private void OnDeleteButtonPressed_Signal()
	{
		DayEventDisplay display = _globalData.GetSelectedDisplay();

		if (display == null) return;
		
		_eventData = display.GetDateEventData();
		
		// ICalFileReader.RemoveDateEvent(_eventData);
	}
}
