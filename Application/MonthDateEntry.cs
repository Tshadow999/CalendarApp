using Godot;

public partial class MonthDateEntry : Control
{
	[Export] private Label DayNumberLabel;

	public override void _Ready()
	{
		SetLabelDate((int)GD.RandRange(1, 31), 1, 1);
	}

	public void SetLabelDate(int day, int month, int year)
	{
		DayNumberLabel.Text = $"{day}";
	}
}
