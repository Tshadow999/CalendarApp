using Godot;

public partial class MonthDateEntry : Control
{
	[Export] private Label DayNumberLabel;
	[Export] private Panel Background;

	public override void _Ready()
	{
		SetLabelDate((int)GD.RandRange(1, 31), 1, 1);
	}

	public void SetLabelDate(int day, int month, int year)
	{
		DayNumberLabel.Text = $"{day}";
	}

	public override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
		{
			
			Background.Modulate = new Color(1, 0, 0);
		}
	}
}
