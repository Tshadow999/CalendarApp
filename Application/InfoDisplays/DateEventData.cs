using System;

public struct DateEventData
{
    public string Name;
    public string Description;
    public string Location;
    
    public DateTime StartDate;
    public DateTime EndDate;

    public static DateEventData Empty => default;

    public DateEventData()
    {
        Name = Description = Location = "";
        StartDate = EndDate = new DateTime();
    }
    
    public static bool operator== (DateEventData lhs, DateEventData rhs)
    {
        return (lhs.StartDate == rhs.StartDate && lhs.EndDate == rhs.EndDate && lhs.Name == rhs.Name);
    }
    
    public static bool operator!= (DateEventData lhs, DateEventData rhs) => !(lhs == rhs);

    public override string ToString()
    {
        return $"{Name}\n{Description}\nLocation:{Location}\nFrom:{StartDate}\nUntil:{EndDate}\n";
    }
}
