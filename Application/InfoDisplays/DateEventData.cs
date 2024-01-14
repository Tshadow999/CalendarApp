using System;

public struct DateEventData
{
    public string Name;
    public string Description;
    public string Location;

    public bool Editable;
    
    public DateTime StartDate;
    public DateTime EndDate;

    public static DateEventData Empty => default;

    public DateEventData()
    {
        Name = Description = Location = "";
        StartDate = EndDate = new DateTime();
        Editable = true;
    }

    public DateEventData(string name)
    {
        Name = name;
        Description = Location = "";
        StartDate = EndDate = new DateTime();
        Editable = true;
    }
    
    public static bool operator== (DateEventData lhs, DateEventData rhs) => lhs.StartDate == rhs.StartDate && lhs.EndDate == rhs.EndDate && lhs.Name == rhs.Name;

    public static bool operator!= (DateEventData lhs, DateEventData rhs) => !(lhs == rhs);
    
    public bool Equals(DateEventData other) => Name == other.Name && StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate);

    public override bool Equals(object obj) => obj is DateEventData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Name, StartDate, EndDate);


    public override string ToString() => $"{Name}\n{Description}\nLocation:{Location}\nFrom:{StartDate}\nUntil:{EndDate}\nEditable:{Editable}";
}
