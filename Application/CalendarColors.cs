using System;
using Godot;
using Godot.Collections;
using System.IO;
using System.Linq;

public partial class CalendarColors : Node
{
    private const string COLOR_DIR = "/Colors";

    private const string defaultValues =
        "{\n  \"Exam\": \"730084\",\n  \"Lectorial\": \"d65300\",\n  \"Lecture\": \"d60000\",\n  \"Lecture Online\": \"7f3100\",\n  \"Other\": \"ffe300\",\n  \"Other Non-Educational\": \"ffe300\",\n  \"Other Online\": \"ffe300\",\n  \"Practical\": \"0f852d\",\n  \"Presentation\": \"9eff00\",\n  \"Project supervised\": \"311300\",\n  \"Project unsupervised\": \"15dc11\",\n  \"Q&A\": \"7f7f7f\",\n  \"Self study supervised\": \"320083\",\n  \"Self study unsupervised\": \"2ce7c8\",\n  \"Tutorial\": \"2c48e8\"\n}";
    private static Dictionary<string, string> _colorTypeDict;

    private static string _dirPath;
    private static string _filePath;

    public override void _Ready()
    {
        _colorTypeDict = new Dictionary<string, string>();
        
        _dirPath = OS.GetUserDataDir() + COLOR_DIR;

        if (!Directory.Exists(_dirPath)) Directory.CreateDirectory(_dirPath);

        _filePath = _dirPath + "/colors.json";

        // if there is an 'empty' file we want to reset
        if (File.Exists(_filePath) && File.OpenRead(_filePath).Length > 10)
        {
            LoadColorDictionary(_filePath);
        }
        else
        {
            _colorTypeDict = (Dictionary<string, string>) Json.ParseString(defaultValues);
            SaveColorDictionary(_filePath);
        }
    }

    private void LoadColorDictionary(string filePath)
    {
        try
        {
            string readableString = File.ReadAllText(filePath);
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
            // GD.Print($"t:{type}, c:{c}, b:True");
            return true;
        }
        c = Colors.White;
        // GD.Print($"t:{type}, c:{c}, b:False");
        return false;
    }

}