using System;
using Godot;

public partial class EditPopup : PanelContainer
{
	[Export] private LineEdit NameEdit;
	
	[Export] private LineEdit LocationEdit;
	[Export] private LineEdit DescriptionEdit;
	
	[Export] private HBoxContainer StartDateContainer;
	[Export] private HBoxContainer EndDateContainer;
	
	private Label _startDateLabel;
	private Label _startTimeLabel;
	
	private Label _endDateLabel;
	private Label _endTimeLabel;
	
	private DateEventData _eventData;
	private DateEventData _editedData;

	public override void _Ready()
	{
		_startDateLabel = StartDateContainer.GetChild<Label>(0); // First child 
		_startTimeLabel = StartDateContainer.GetChild<Label>(2); // Last child 
		
		_endDateLabel = EndDateContainer.GetChild<Label>(0); // First child 
		_endTimeLabel = EndDateContainer.GetChild<Label>(2); // Last child 
		
		_eventData = new DateEventData
		{
			Name = "Testing Name",
			Location = "Location test",
			Description = "Some important description",
			StartDate = new DateTime(2023, 12, 17, 12, 0, 0),
			EndDate = new DateTime(2023, 12, 17, 18, 0, 0)
		};
		
		Initialize(_eventData);
	}
	
	public void Initialize(DateEventData dateEvent)
	{
		_eventData = dateEvent;
		
		if (Equals(_eventData, DateEventData.Empty)) return;

		_editedData = _eventData;
		
		_startDateLabel.Text = $"{_eventData.StartDate.DayOfWeek} {_eventData.StartDate.Day} {DateTimeHelper.GetMonthFromIndex(_eventData.StartDate.Month)} {_eventData.StartDate.Year}";
		_endDateLabel.Text = $"{_eventData.StartDate.DayOfWeek} {_eventData.EndDate.Day} {DateTimeHelper.GetMonthFromIndex(_eventData.EndDate.Month)} {_eventData.EndDate.Year}";

		_startTimeLabel.Text = _eventData.StartDate.TimeOfDay.ToString(@"hh\:mm");
		_endTimeLabel.Text = _eventData.EndDate.TimeOfDay.ToString(@"hh\:mm");

		NameEdit.Text = _eventData.Name;
		LocationEdit.Text = _eventData.Location;
		DescriptionEdit.Text = _eventData.Description;
	}
	
	private void AllDayToggled_Signal(bool toggleOn)
	{
		_startTimeLabel.Visible = !toggleOn;
		_endTimeLabel.Visible = !toggleOn;
	}
	
	private void OnTitleEditTextChanged_Signal(string editedText) => _editedData.Name = editedText;
	
	private void OnLocationChanged_Signal(string editedText) => _editedData.Location = editedText;

	private void OnDescriptionChanged_Signal(string editedText) => _editedData.Description = editedText;

	private void OnBackButtonPressed_Signal() => Visible = false;

	private void OnConfirmButtonPressed_Signal()
	{
		if (_eventData != _editedData) ICalFileReader.EditDateEvent(_eventData, _editedData);
		
		OnBackButtonPressed_Signal();
	}
}
