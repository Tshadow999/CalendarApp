using System;
using Godot;
using Godot.Collections;
using System.IO;
using System.Linq;

public partial class CalendarColors : Node
{
    private const string COLOR_DIR = "/Colors";
    private static Dictionary<string, string> _colorTypeDict;

    private static string _dirPath;
    private static string _filePath;

    public override void _Ready()
    {
        _colorTypeDict = new Dictionary<string, string>();
        
        // For testing saving and loading
        // _colorTypeDict.Add("EY1", Colors.Red.ToHtml());
        // _colorTypeDict.Add("EY2", Colors.BlueViolet.ToHtml());
        // _colorTypeDict.Add("EY3", Colors.Lime.ToHtml());
        
        _dirPath = OS.GetUserDataDir() + COLOR_DIR;

        if (!Directory.Exists(_dirPath)) Directory.CreateDirectory(_dirPath);

        _filePath = _dirPath + "/colors.json";

        if (File.Exists(_filePath))
        {
            LoadColorDictionary(_filePath);
        }
        else
        {
            FileStream fs = File.Create(_filePath);
            fs.Close();
            
            SaveColorDictionary(_filePath);
        }
    }

    private void LoadColorDictionary(string filePath)
    {
        try
        {
            string readableString = File.ReadAllText(filePath);
            GD.Print(readableString);
            _colorTypeDict = (Dictionary<string, string>) Json.ParseString(readableString);
        }
        catch (Exception ex)
        {
            GD.Print("Error loading color dictionary: " + ex.Message);
        }
    }
    
    private void SaveColorDictionary(string filePath)
    {
        try
        {
            string json = Json.Stringify(_colorTypeDict);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            GD.Print("Error saving color dictionary: " + ex.Message);
        }
    }

    public static bool GetColor(string type, out Color c)
    {
        if (_colorTypeDict.TryGetValue(type, out string s))
        {
            c = Color.FromHtml(s);
            GD.Print($"t:{type}, c:{c}, b:True");
            return true;
        }
        c = Colors.White;
        GD.Print($"t:{type}, c:{c}, b:False");
        return false;
    }

}