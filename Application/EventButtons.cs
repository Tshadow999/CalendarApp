using Godot;

public partial class EventButtons : HBoxContainer
{
	private DateEventData _eventData;
	private DateEventData _editedData;
	
	private void OnAddButtonPressed_Signal()
	{
		ICalFileReader.AddDateEvent(_eventData);
	}

	private void OnEditButtonPressed_Signal()
	{
		ICalFileReader.EditDateEvent(_eventData, _editedData);
	}

	private void OnDeleteButtonPressed_Signal()
	{
		ICalFileReader.RemoveDateEvent(_eventData);
	}
}
