using Godot;

public partial class DayEventDisplay : Control
{
	[Export] private Label NameLabel;
	[Export] private Label TimeLabel;
	[Export] private Label LocationLabel;

	private DateEvent _event;

	public override void _Ready()
	{
		SetLabelTexts();
	}

	public void SetDateEvent(DateEvent dateEvent)
	{
		_event = dateEvent;
		SetLabelTexts();
	}

	private void SetLabelTexts()
	{
		NameLabel.Text = _event.Name;
		LocationLabel.Text = _event.Location;
		TimeLabel.Text = $"{_event.StartDate.TimeOfDay} - {_event.EndDate.TimeOfDay}";
	}
	
}
