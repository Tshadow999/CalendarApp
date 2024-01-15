using System.Collections.Generic;
using System;
using Godot;

public partial class MonthDateEntry : Control
{
	private const string THEME_VARIANT = "SelectedMonthEntry";

	public static Action<MonthDateEntry> OnClick;
	
	[Export] private Label DayNumberLabel;
	[Export] private Panel SelectedDayBackground;
	[Export] private VBoxContainer DayEventContainer;
	[Export(PropertyHint.File, "*.tscn")] private string DayEventScenePath;
	
	private int _year;
	private int _month;
	private int _day;
	
	private PackedScene _dayEventScene;

	private List<DateEventData> _dateEvents;

	private float _lastTapTime;

	public override void _Ready()
	{
		_dayEventScene = ResourceLoader.Load<PackedScene>(DayEventScenePath);

		_dateEvents = new List<DateEventData>();
		
		OnClick += Clicked;
	}

	private void Clicked(MonthDateEntry other)
	{
		Set("theme_type_variation", this == other ? THEME_VARIANT : "");
	}

	private void OnGuiInput_Signal(InputEvent @event)
	{
		if (@event is InputEventScreenTouch { Pressed: true })
		{
			OnClick?.Invoke(this);
			
			GlobalData.SetSelectedDateTime(new DateTime(_year, _month, _day));

			// Double tap logic
			float currentTimeSec = Time.GetTicksMsec() / 1000.0f;

			bool doubleTapDone = currentTimeSec - _lastTapTime <= 0.5f;
			
			_lastTapTime = doubleTapDone ? 0f : currentTimeSec;
			
			if (doubleTapDone) GlobalData.OpenPopup();
		}
	}

	public void ClearSelection() => Set("theme_type_variation", "");

	private void CreateDayEvent(DateEventData dateEventData)
	{
		DayEvent dayEvent = _dayEventScene.Instantiate<DayEvent>();

		DayEventContainer.AddChild(dayEvent);
		
		dayEvent.SetEvent(dateEventData);

		if (!CalendarColors.GetColor(dateEventData.Description, out Color c))
		{
			GD.Print($"COLOR NOT FOUND: {dateEventData.Description}");
		}
		dayEvent.SetColor(c);
		
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

	public List<DateEventData> GetDateEvents() => _dateEvents;
}
