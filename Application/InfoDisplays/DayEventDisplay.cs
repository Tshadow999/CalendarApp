using Godot;

public partial class DayEventDisplay : Control
{
	[Export] private Label NameLabel;
	[Export] private Label TimeLabel;
	[Export] private Label LocationLabel;

	private DateEventData _eventData;

	[Export] private float DoubleTapThreshold = 0.5f; 
	private float _lastTapTime;
	
	private Application _applicationNode;
	
	public override void _Ready()
	{
		_applicationNode = GetNode<Application>("/root/Application");
		UpdateLabelTexts();
	}

	public void SetDateEvent(DateEventData dateEventData)
	{
		_eventData = dateEventData;
		Name = $"{_eventData.Name}_{_eventData.StartDate.TimeOfDay}_{_eventData.EndDate.TimeOfDay}";
		UpdateLabelTexts();
	}

	public DateEventData GetDateEventData() => _eventData;

	private void UpdateLabelTexts()
	{
		NameLabel.Text = _eventData.Name;
		LocationLabel.Text = _eventData.Location;
		TimeLabel.Text = $"{_eventData.StartDate.TimeOfDay} - {_eventData.EndDate.TimeOfDay}";
	}

	private void OnGuiInput_Signal(InputEvent @event)
	{
		if (@event is not InputEventScreenTouch { Pressed: true }) return;

		GlobalData data = GetNode<GlobalData>("/root/GlobalData");
		data.SetSelectedDisplay(this);
		
		Set("theme_type_variation", "SelectedMonthEntry");
		
		float currentTimeSec = Time.GetTicksMsec() / 1000.0f;

		bool doubleTapDone = currentTimeSec - _lastTapTime <= DoubleTapThreshold;

		_lastTapTime = doubleTapDone ? 0f : currentTimeSec;
		_applicationNode.ToggleEditPopup(doubleTapDone, _eventData);
	}
}
