using System;
using Godot;

public partial class MonthDateEntry : Control
{
	[Export] private Label DayNumberLabel;
	[Export] private Panel Background;
	[Export] private Panel SelectedDayBackground;
	[Export] private VBoxContainer DayEventContainer;
	[Export(PropertyHint.File, "*.tscn")] private string DayEventScenePath;
	
	private int _year;
	private int _month;
	private int _day;

	private PackedScene _dayEventScene;

	public override void _Ready()
	{
		_dayEventScene = ResourceLoader.Load<PackedScene>(DayEventScenePath);

		for (int i = 0; i < 9; i++)
		{
			CreateDayEvent();
		}
	}

	private void CreateDayEvent()
	{
		DayEvent dayEvent = _dayEventScene.Instantiate() as DayEvent;

		DayEventContainer.AddChild(dayEvent);

		// TODO: GET DATE EVENT FOR THIS DATE!
		dayEvent.SetEvent(new DateEvent());
		dayEvent.SetColor(Colors.IndianRed);
	}

	public void SetLabelDate(int day, int month, int year)
	{
		_year = year;
		_month = month;
		_day = day;
		HandleSelectedDay(day, month, year);

		DayNumberLabel.Text = $"{day}";
	}

	private void HandleSelectedDay(int day, int month, int year)
	{
		SelectedDayBackground.Visible = DateTime.Today.Equals(new DateTime(year, month, day));
		Color labelColor = DateTime.Today.Equals(new DateTime(year, month, day)) ? Colors.Black : Colors.White;
		DayNumberLabel.Set("theme_override_colors/font_color", labelColor);
	}

	public override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
		{
			//TODO: SHOW today events in the display
			Background.Modulate = new Color(1, 0, 0);
		}
	}
}
