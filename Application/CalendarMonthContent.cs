using System;
using Godot;
using Godot.Collections;

public partial class CalendarMonthContent : VBoxContainer
{
	[Export] private VBoxContainer WeekNumberContainer;
	private Array<Label> _weekNumberLabels;
	
	[Export] private GridContainer CalendarGridContainer;
	private Array<MonthDateEntry> _monthDateEntries;

	public override void _Ready()
	{
		Array<Node> weekNumChildren = WeekNumberContainer.GetChildren();

		_weekNumberLabels = new Array<Label>();
		
		foreach (Node child in weekNumChildren)
		{
			_weekNumberLabels.Add((Label)child);
		}
		
		Array<Node> gridChildren = CalendarGridContainer.GetChildren();

		_monthDateEntries = new Array<MonthDateEntry>();
		foreach (Node child in gridChildren)
		{
			_monthDateEntries.Add((MonthDateEntry)child);
		}

		SetMonthDayNumbers(DateTime.Today.Month, DateTime.Today.Year);
	}

	private void SetMonthDayNumbers(int month, int year)
	{
		int daysInCurrentMonth = DateTime.DaysInMonth(year, month);
		
		DateTime firstDayOfMonth = new DateTime(year, month, 1);
		DayOfWeek startDayOfWeek = firstDayOfMonth.DayOfWeek;
		
		int daysFromPreviousMonthToDisplay = (int) startDayOfWeek - 1;

		if (daysFromPreviousMonthToDisplay == -1) daysFromPreviousMonthToDisplay = 6;
		
		int startDayNumber = 1;
		
		int previousMonthYear = year;
		int previousMonth = month - 1;
		if (previousMonth < 1)
		{
			previousMonth = 12;
			previousMonthYear--;
		}

		for (int i = 0; i < _monthDateEntries.Count; i++)
		{
			int dayNumber;
			MonthDateEntry dateEntry = _monthDateEntries[i];
			
			if (i < 7 && i % 7 < daysFromPreviousMonthToDisplay)
			{
				// this only runs for days in the previous month
				int daysInPreviousMonth = DateTime.DaysInMonth(previousMonthYear, previousMonth);
				dayNumber = daysInPreviousMonth - daysFromPreviousMonthToDisplay + (i % 7) + 1;
				dateEntry.SetLabelDate(dayNumber, daysInPreviousMonth, previousMonthYear);
			}
			else
			{
				// normal incrementing
				dayNumber = startDayNumber++;
				dateEntry.SetLabelDate(dayNumber, month, year);
				
				// reset the dayNumbers for the next month
				if (dayNumber > daysInCurrentMonth)
				{
					dayNumber = 1;
					startDayNumber = 2;

					// if next month is also next year
					if (month == 12)
					{
						dateEntry.SetLabelDate(dayNumber, 1, year + 1);
					}
					else
					{
						dateEntry.SetLabelDate(dayNumber, month, year);
					}
				}
			}
		}
	}

	private void OnCarouselChanged_Signal(int month, int year)
	{
		SetMonthDayNumbers(month, year);
	}
	

	private void SetWeekNumberLabelText(int initialWeekNumber)
	{
		for (int i = 0; i < _weekNumberLabels.Count; i++)
		{
			_weekNumberLabels[i].Text = $"{initialWeekNumber + i}";
		}
	}
}
