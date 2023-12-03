using System;
using Godot;

public partial class CarouselContainer : HBoxContainer
{
	[Signal] public delegate void OnCarouselChangedEventHandler(int selectedMonth, int selectedYear);
	[Export] private Label DisplayText;

	private int _selectedMonth;
	private int _selectedYear;

	public override void _Ready()
	{
		_selectedMonth = DateTime.Today.Month;
		_selectedYear = DateTime.Today.Year;
		SetDisplayText();
	}

	private void SetDisplayText() => DisplayText.Text = DateTimeHelper.GetMonthFromIndex(_selectedMonth);

	private void HandleButtonPress(int monthChange)
	{
		if (monthChange == 1 && _selectedMonth == 12)
		{
			_selectedMonth = 1;
			_selectedYear++;
		}
		else if (monthChange == -1 && _selectedMonth == 1)
		{
			_selectedMonth = 12;
			_selectedYear--;
		}
		else
		{
			_selectedMonth += monthChange;
		}

		SetDisplayText();
		EmitSignal(SignalName.OnCarouselChanged, _selectedMonth, _selectedYear);
	}
	
	private void OnLeftButtonPressed_Signal() => HandleButtonPress(-1);

	private void OnRightButtonPressed_Signal() => HandleButtonPress(1);
}
