using Godot;

public partial class DayEventDisplay : Control
{
	[Export] private Label NameLabel;
	[Export] private Label TimeLabel;
	[Export] private Label LocationLabel;

	private DateEventData _eventData;

	[Export] private float DoubleTapThreshold = 0.5f; 
	private float _lastTapTime;
	
	public override void _Ready()
	{
		UpdateLabelTexts();
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventScreenTouch { Pressed: true })
		{
			float currentTimeSec = Time.GetTicksMsec() / 1000.0f;

			string name = Name;
			if (name.Contains("Control")) return;
			// Check if the time since the last tap is within the double tap threshold
			if (currentTimeSec - _lastTapTime <= DoubleTapThreshold)
			{
				// Reset the last tap time
				_lastTapTime = 0f;

				//TODO: Have a popup to edit this info
				
				
			}
			else
			{
				// Update the last tap time for the next comparison
				_lastTapTime = currentTimeSec;
			}
		}
	}

	public void SetDateEvent(DateEventData dateEventData)
	{
		_eventData = dateEventData;
		UpdateLabelTexts();
	}

	private void UpdateLabelTexts()
	{
		NameLabel.Text = _eventData.Name;
		LocationLabel.Text = _eventData.Location;
		TimeLabel.Text = $"{_eventData.StartDate.TimeOfDay} - {_eventData.EndDate.TimeOfDay}";
	}
	
}
