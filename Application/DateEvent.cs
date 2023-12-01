using System;

public struct DateEvent
{
    public string Name;
    public string Description;
    public string Location;
    
    public DateTime StartDate;
    public DateTime EndDate;


    public override string ToString()
    {
        return $"{Name}\n{Description}\nLocation:{Location}\nStart:{StartDate}\nEnd:{EndDate}\n";
    }
}
