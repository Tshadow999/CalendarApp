using System;
using Godot;

public partial class EditPopup : PanelContainer
{
	[Export] private LineEdit NameEdit;
	
	[Export] private LineEdit LocationEdit;
	[Export] private LineEdit DescriptionEdit;
	
	[Export] private HBoxContainer StartDateContainer;
	[Export] private HBoxContainer EndDateContainer;

	[Export] private TimeSelector Selector;
	
	private Label _startDateLabel;
	private Label _startTimeLabel;
	
	private Label _endDateLabel;
	private Label _endTimeLabel;
	
	private DateEventData _eventData;
	private DateEventData _editedData;

	private bool _isNewEvent;

	public override void _Ready()
	{
		_startDateLabel = StartDateContainer.GetChild<Label>(0); // First child 
		_startTimeLabel = StartDateContainer.GetChild<Label>(2); // Last child 
		
		_endDateLabel = EndDateContainer.GetChild<Label>(0); // First child 
		_endTimeLabel = EndDateContainer.GetChild<Label>(2); // Last child 
	}
	
	public void Initialize(DateEventData dateEvent, bool isNewEvent = false)
	{
		_isNewEvent = isNewEvent;
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
		
		Visible = true;
		Selector.Visible = false;
	}
	
	private void AllDayToggled_Signal(bool toggleOn)
	{
		_startTimeLabel.Visible = !toggleOn;
		_endTimeLabel.Visible = !toggleOn;

		if (!toggleOn) return;

		DateTime selected = GlobalData.GetSelectedDateTime();
		
		_editedData.StartDate = new DateTime(selected.Year, selected.Month, selected.Day, 0, 0, 0);
		_editedData.EndDate = new DateTime(selected.Year, selected.Month, selected.Day, 23, 59, 59);
	}
	
	private void OnTitleEditTextChanged_Signal(string editedText) => _editedData.Name = editedText;
	
	private void OnLocationChanged_Signal(string editedText) => _editedData.Location = editedText;

	private void OnDescriptionChanged_Signal(string editedText) => _editedData.Description = editedText;

	private void OnBackButtonPressed_Signal() => Visible = false;

	private void OnConfirmButtonPressed_Signal()
	{
		if (_isNewEvent)
		{
			ICalFileReader.AddDateEvent(_editedData);
			
			OnBackButtonPressed_Signal();
			return;
		}
		
		if (_eventData != _editedData) ICalFileReader.EditDateEvent(_eventData, _editedData);
		
		OnBackButtonPressed_Signal();
	}

	private void OnStartDateGuiInput_Signal(InputEvent @event)
	{
		if (@event is InputEventScreenTouch { Pressed: true } eventScreenTouch)
		{
			Selector.Init(this, true);
		}
	}

	private void OnEndDateGuiInput_Signal(InputEvent @event)
	{
		if (@event is InputEventScreenTouch { Pressed: true } eventScreenTouch)
		{
			Selector.Init(this, false);
		}
	}

	public void SetTime(Vector2I time, bool startTime)
	{
		if (startTime)
		{
			_startTimeLabel.Text = $"{time.X:00}:{time.Y:00}";
			_editedData.StartDate = new DateTime(_editedData.StartDate.Year, _editedData.StartDate.Month,
				_editedData.StartDate.Day, time.X, time.Y, 0);
		}
		else
		{
			_endTimeLabel.Text = $"{time.X:00}:{time.Y:00}";
			_editedData.EndDate = new DateTime(_editedData.EndDate.Year, _editedData.EndDate.Month,
				_editedData.EndDate.Day, time.X, time.Y, 0);
		}
	}
}
