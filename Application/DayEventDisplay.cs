using Godot;

public partial class DayEventDisplay : Control
{
	[Export] private Label NameLabel;
	[Export] private Label TimeLabel;
	[Export] private Label LocationLabel;

	private DateEventData _eventData;

	public override void _Ready()
	{
		UpdateLabelTexts();
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
