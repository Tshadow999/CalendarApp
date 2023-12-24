using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public partial class ICalFileReader : Node
{
	[Export] private HttpRequest RequestNode;
	public static event Action OnReady;
	
	private const string utwenteRoosterICalLink =
		"https://rooster.utwente.nl/ical?6569c5eb&group=true&eu=czI1NTYxNTQ=&h=V5QsZ8N6rV__HKkSVTgWhYTi7Caox_owB_BHwVSbRCM=";
	
	private const string DATE_TIME_FORMAT = "yyyyMMddTHHmmss";
	
	private const string ICalDirectory = "/ICS Files";

	private const string BEGIN = "BEGIN";
	private const string END = "END";
	private const string EVENT = "VEVENT";
	private const string DTSTART = "DTSTART";
	private const string DTEND = "DTEND";
	private const string LOCATION = "LOCATION";
	private const string SUMMARY = "SUMMARY";
	private const string DESCRIPTION = "DESCRIPTION";
	
	private static List<DateEventData> _dateEvents;
	
	private Label _debugLabel;

	private string _totalPath;

	private static DateEventData _currentEventData;

	private static bool _previousLineWasLocation;
		
	
	private readonly Dictionary<string, Action<string>> _propertyActionsDict = new Dictionary<string, Action<string>>
	{
		{ BEGIN, ParseBegin },
		{ END, ParseEnd },
		{ DTSTART, ParseStartDate },
		{ DTEND, ParseEndDate},
		{ LOCATION, ParseLocationLine },
		{ SUMMARY, ParseSummaryLine },
		{ DESCRIPTION, ParseDescription }
	};

	public override void _Ready()
	{
		_totalPath = $"{OS.GetUserDataDir()}{ICalDirectory}";

		Directory.CreateDirectory(_totalPath);
		
		RequestNode.DownloadFile = $"{_totalPath}/rooster.ics";
		
		_dateEvents = new List<DateEventData>();
		// Need to wait a bit before going on.
		GetTree().CreateTimer(0.15f).Timeout += OnTimeout;
	}

	private void OnTimeout()
	{
		_debugLabel = Debugger.GetLabel();
		
		RequestNode.RequestCompleted += OnHttpRequestCompleted;
		try
		{
			RequestNode.Request(utwenteRoosterICalLink);
		}
		catch (Exception e)
		{
			_debugLabel.Visible = true;
			_debugLabel.Text = $"[GET]: {e.Message}";
		}
	}

	private void OnHttpRequestCompleted(long result, long responsecode, string[] headers, byte[] body)
	{
		ReadICalFiles();
	}
	
	private void ReadICalFiles()
	{
		try
		{
			_debugLabel.Text = $"OSPath:{ProjectSettings.GlobalizePath(_totalPath)}\n";			
			_debugLabel.Text += $"path:{_totalPath}\n";

			string[] iCalFiles = Directory.GetFiles(_totalPath);
			
			foreach (string filePath in iCalFiles)
			{
				string properPath = filePath.Replace("\\", "/");
				_debugLabel.Text += $"File:{properPath}\n";
				ReadFile(properPath);
			}
		}
		catch (Exception e)
		{
			// _debugLabel.Visible = true;
			// _debugLabel.Text += $"[PATH]: {e.Message}\n";
			GD.Print($"[PATH]: {e.Message}");
		}
		
		OnReady?.Invoke();
	}

	private void ReadFile(string globalPathICalFile)
	{
		string[] lines = File.ReadAllLines(globalPathICalFile);

		foreach (string line in lines)
		{
			if (_previousLineWasLocation)
			{
				// Check if the locations didnt fit on one line. 
				// This is likely not to happen 
				_previousLineWasLocation = false;
				
				if (line.StartsWith(' '))
				{
					// The first character is always a space, so we take the line from the second character
					// Also remove the backslashes '\' which are there
					_currentEventData.Location += line.Replace("\\", "")[1..];
				}
			} 
			else if (_propertyActionsDict.TryGetValue(line.Split(":")[0].Split(";")[0], out Action<string> action))
			{
				action.Invoke(line);
			}
		}
	}
	
	private static void ParseBegin(string line)
	{
		if (!line.Contains(EVENT)) return;
		
		_currentEventData = new DateEventData();
	}
	
	private static void ParseEnd(string line)
    {
    	if (!line.Contains(EVENT)) return;
    	_dateEvents.Add(_currentEventData);
	} 
	
	private static void ParseEndDate(string line) => _currentEventData.EndDate = ParseICSDateTime(line);
    
	private static void ParseStartDate(string line) => _currentEventData.StartDate = ParseICSDateTime(line);

	private static void ParseSummaryLine(string line)
	{
		string result = line.Split(":")[1];
		result = Regex.Replace(result, @"\d+$", "");
		_currentEventData.Name = result;
	} 

	private static void ParseDescription(string line)
	{
		const string pattern = @"Type:\s*(.*?)(?:\\n|$)";
		
		Match match = Regex.Match(line, pattern);

		if (match.Success)
		{
			_currentEventData.Description = match.Groups[1].Value;
		}
		else
		{
			GD.Print("[DESCRIPTION] No match found");
		}
	} 

	private static void ParseLocationLine(string line)
	{
		// template: 'LOCATION:some_location\, some_other_location\, etc'
		string location = line.Split(":")[1];
		location = location.Replace("\\", "");
		// Location can be empty, so just say Unknown
		_currentEventData.Location = location.Length == 0 ? "Unknown" : location;
		_previousLineWasLocation = true;
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

	public static void AddDateEvent(DateEventData data)
	{
		if (_dateEvents.Contains(data))
		{
			GD.Print("DATA ALREADY IN LIST");
			GD.Print(data);
			return;
		}

		_dateEvents.Add(data);

		OnReady?.Invoke();
	}
	
	public static void EditDateEvent(DateEventData oldData, DateEventData editedData)
	{
		if (!_dateEvents.Contains(oldData))
		{
			GD.Print("DATA NOT IN LIST");
			GD.Print(oldData);
			return;
		}
		
		int index = _dateEvents.IndexOf(oldData);
		_dateEvents[index] = editedData;
		
		OnReady?.Invoke();
	}

	public static void RemoveDateEvent(DateEventData data)
	{
		if (!_dateEvents.Contains(data))
		{
			GD.Print("DATA NOT IN LIST");
			GD.Print(data);
			return;
		}
		_dateEvents.Remove(data);

		OnReady?.Invoke();
	}
}
