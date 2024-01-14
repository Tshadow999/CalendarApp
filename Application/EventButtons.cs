using Godot;

public partial class EventButtons : HBoxContainer
{
	private void OnAddButtonPressed_Signal() => GlobalData.OpenPopup();

	private void OnEditButtonPressed_Signal()
	{
		DayEventDisplay selectedDisplay = GlobalData.GetSelectedDisplay();

		if (selectedDisplay == null || !selectedDisplay.GetDateEventData().Editable) return;
		
		GlobalData.OpenPopup(selectedDisplay.GetDateEventData());
	}

	private void OnDeleteButtonPressed_Signal()
	{
		DayEventDisplay display = GlobalData.GetSelectedDisplay();

		if (display == null || !display.GetDateEventData().Editable) return;

		GD.Print(display.GetDateEventData());
		
		ICalFileReader.RemoveDateEvent(display.GetDateEventData());
	}
}
