using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FileAccess = Godot.FileAccess;

public partial class ICalFileReader : Node
{
	[Signal] public delegate void OnReadyEventHandler();
	private const string DATE_TIME_FORMAT = "yyyyMMddTHHmmss";
	
	[Export(PropertyHint.Dir, "*ICS*")] private string ICalDirectory;

	private static List<DateEventData> _dateEvents;

	public override void _Ready()
	{
		_dateEvents = new List<DateEventData>();
	}
	
	public void ReadICalFiles()
	{
		string[] iCalFiles = Directory.GetFiles(ProjectSettings.GlobalizePath(ICalDirectory));
		
		foreach (string filePath in iCalFiles)
		{
			string properPath = filePath.Replace("\\", "/");
			ReadFile(properPath);
		}

		EmitSignal(SignalName.OnReady);
	}

	private void ReadFile(string globalPathICalFile)
	{
		
		GD.Print("READING FILE");
		string[] lines = File.ReadAllLines(globalPathICalFile);
		GD.Print("DONE READING FILE");

		bool previousLineWasLocation = false;
		DateEventData currentEventData = new DateEventData();
		foreach (string line in lines)
		{
			if (previousLineWasLocation)
			{
				previousLineWasLocation = false;
				if (line.StartsWith(' '))
				{
					// The first character is always a space, so we take the line from the second character
					// Also remove the backslashes '\' which are there
					currentEventData.Location += line.Replace("\\", "")[1..];
				}
			}
			else if (line.Contains("BEGIN:VEVENT"))
			{
				currentEventData = new DateEventData();
			}
			else if (line.Contains("END:VEVENT"))
			{
				_dateEvents.Add(currentEventData);
				// if (Debugger.IsDebugging) GD.Print(currentEvent);
			}
			else if (line.Contains("DTSTART"))
			{
				currentEventData.StartDate = ParseICSDateTime(line);
			}
			else if (line.Contains("DTEND"))
			{
				currentEventData.EndDate = ParseICSDateTime(line);
			}
			else if (line.Contains("LOCATION"))
			{
				// template: 'LOCATION:some_location\, some_other_location\, etc'
				string location = line.Split(":")[1];
				location = location.Replace("\\", "");
				// Location can be empty, so just say Unknown
				currentEventData.Location = location.Length == 0 ? "Unknown" : location;
				previousLineWasLocation = true;
			}
			else if (line.Contains("SUMMARY"))
			{
				// template: 'SUMMARY:some_data'
				currentEventData.Name = line.Split(":")[1];
			}
			else if (line.Contains("DESCRIPTION"))
			{
				// template: 'DESCRIPTION:type: some_date\n\n_some_more_data\n\n_etc'
				string s = line.Split(": ")[1].Split("\\")[0];
				currentEventData.Description = s;
			}
			// if (Debugger.IsDebugging) GD.Print(line);
		}
	}

	private static DateTime ParseICSDateTime(string line)
	{
		return DateTime.ParseExact(line.Split(":")[1], DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
	}

	public static List<DateEventData> GetDateEventsOnDay(int day, int month, int year)
	{
		DateTime dayInQuestion = new DateTime(year, month, day);

		return _dateEvents.Where(dateEvent => dateEvent.StartDate.Date == dayInQuestion.Date).ToList();
	}
}
