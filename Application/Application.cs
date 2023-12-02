using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Godot;

public partial class Application : Control
{	
	private const string QUIT_KEY = "QUIT";

	private const string DATE_TIME_FORMAT = "yyyyMMddTHHmmss";
	
	[Export(PropertyHint.File, "*.ics")] private string iCalFilePath;

	private List<DateEvent> _dateEvents;

	public override void _Ready()
	{
		Debugger.IsDebugging = true;

		_dateEvents = new List<DateEvent>();
		
		ReadFile(iCalFilePath);
	}

	private void ReadFile(string localPathToICSFile)
	{
		string[] lines = File.ReadAllLines(ProjectSettings.GlobalizePath(localPathToICSFile));

		bool checkNextLine = false;
		bool previousLineWasLocation = false;
		DateEvent currentEvent = new DateEvent();
		
		foreach (string line in lines)
		{
			if (previousLineWasLocation)
			{
				previousLineWasLocation = false;
				if (checkNextLine && line.StartsWith(' '))
				{
					// The first character is always a space, so we take the line from the second character
					// Also remove the backslashes '\' which are there
					currentEvent.Location += line.Replace("\\", "")[1..];
					checkNextLine = false;
				}
			}
			else if (line.Contains("BEGIN:VEVENT"))
			{
				currentEvent = new DateEvent();
			}
			else if (line.Contains("END:VEVENT"))
			{
				_dateEvents.Add(currentEvent);
				if (Debugger.IsDebugging) GD.Print(currentEvent);
			}
			else if (line.Contains("DTSTART"))
			{
				currentEvent.StartDate = ParseICSDateTime(line);
			}
			else if (line.Contains("DTEND"))
			{
				currentEvent.EndDate = ParseICSDateTime(line);
			}
			else if (line.Contains("LOCATION"))
			{
				// template: 'LOCATION:some_location\, some_other_location\, etc'
				string location = line.Split(":")[1];
				location = location.Replace("\\", "");
				// Location can be empty, so just say Unknown
				currentEvent.Location = location.Length == 0 ? "Unknown" : location;
				checkNextLine = true;
				previousLineWasLocation = true;
			}
			else if (line.Contains("SUMMARY"))
			{
				// template: 'SUMMARY:some_data'
				currentEvent.Name = line.Split(":")[1];
			}
			else if (line.Contains("DESCRIPTION"))
			{
				// template: 'DESCRIPTION:type: some_date\n\n_some_more_data\n\n_etc'
				string s = line.Split(": ")[1].Split("\\")[0];
				currentEvent.Description = s;
			}
			// if (Debugger.IsDebugging) GD.Print(line);
		}
	}

	private static DateTime ParseICSDateTime(string line)
	{
		return DateTime.ParseExact(line.Split(":")[1], DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsAction(QUIT_KEY)) GetTree().Quit();
	}
}
