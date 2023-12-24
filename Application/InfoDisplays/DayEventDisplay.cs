using Godot;

public partial class DayEventDisplay : Control
{
	[Export] private Label NameLabel;
	[Export] private Label TimeLabel;
	[Export] private Label LocationLabel;

	private DateEventData _eventData;

	[Export] private float DoubleTapThreshold = 0.5f; 
	private float _lastTapTime;

	private bool _hoverMouse;
	
	public override void _Ready()
	{
		UpdateLabelTexts();
	}
	
	public override void _Input(InputEvent @event)
	{
		if (_hoverMouse && @event is InputEventScreenTouch { Pressed: true })
		{
			GD.Print(Name);
			float currentTimeSec = Time.GetTicksMsec() / 1000.0f;

			// string name = Name; // Convert StringName to string
			// if (name.Contains("Control")) return;
			
			// Check if the time since the last tap is within the double tap threshold
			if (currentTimeSec - _lastTapTime <= DoubleTapThreshold)
			{
				// Reset the last tap time
				_lastTapTime = 0f;

				Application applicationNode = GetNode<Application>("/root/Application");
				applicationNode.ToggleEditPopup(true, _eventData);
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
		Name = $"{_eventData.Name}";
		UpdateLabelTexts();
	}

	private void UpdateLabelTexts()
	{
		NameLabel.Text = _eventData.Name;
		LocationLabel.Text = _eventData.Location;
		TimeLabel.Text = $"{_eventData.StartDate.TimeOfDay} - {_eventData.EndDate.TimeOfDay}";
	}


	private void OnMouseEntered_Signal() => _hoverMouse = true;

	private void OnMouseExited_Signal() => _hoverMouse = false;
}
