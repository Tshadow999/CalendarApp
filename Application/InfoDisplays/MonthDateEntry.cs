using System.Collections.Generic;
using System;
using Godot;

public partial class MonthDateEntry : Control
{
	public static event Action<MonthDateEntry> OnClick;
	
	[Export] private Label DayNumberLabel;
	[Export] private Panel Background;
	[Export] private Panel SelectedDayBackground;
	[Export] private VBoxContainer DayEventContainer;
	[Export(PropertyHint.File, "*.tscn")] private string DayEventScenePath;
	
	private int _year;
	private int _month;
	private int _day;

	private bool _isSelected = false;

	private PackedScene _dayEventScene;

	private List<DateEventData> _dateEvents;

	public override void _Ready()
	{
		_dayEventScene = ResourceLoader.Load<PackedScene>(DayEventScenePath);

		_dateEvents = new List<DateEventData>();
		
		OnClick += OnClickEntry;
	}

	private void OnClickEntry(MonthDateEntry dateEntry)
	{
		string themeVariant = this == dateEntry ? "SelectedMonthEntry" : "";
		Background.Set("theme_type_variation", themeVariant);
	}

	private void CreateDayEvent(DateEventData dateEventData)
	{
		DayEvent dayEvent = _dayEventScene.Instantiate() as DayEvent;

		DayEventContainer.AddChild(dayEvent);
		
		dayEvent.SetEvent(dateEventData);
		dayEvent.SetColor(Colors.IndianRed);
	}

	public void SetLabelDate(int day, int month, int year)
	{
		_year = year;
		_month = month;
		_day = day;
		HandleSelectedDay(day, month, year);

		DayNumberLabel.Text = $"{day}";
		
		// Remove the old day event children
		foreach (Node child in DayEventContainer.GetChildren())
		{
			DayEventContainer.RemoveChild(child);
			child.QueueFree();
		}
		
		// Create the new day events
		_dateEvents.Clear();
		_dateEvents = ICalFileReader.GetDateEventsOnDay(_day, _month, _year);
		
		foreach (DateEventData dateEvent in _dateEvents)
		{
			CreateDayEvent(dateEvent);
		}
	}

	private void HandleSelectedDay(int day, int month, int year)
	{
		SelectedDayBackground.Visible = DateTime.Today.Equals(new DateTime(year, month, day));
		Color labelColor = DateTime.Today.Equals(new DateTime(year, month, day)) ? Colors.Black : Colors.White;
		DayNumberLabel.Set("theme_override_colors/font_color", labelColor);
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
		{
			OnClick?.Invoke(this);
		}
	}

	public List<DateEventData> GetDateEvents() => _dateEvents;
}
