using System;
using System.Globalization;

public static class DateTimeHelper
{
    private static readonly string[] _monthNames =
    {
        "January", "February", "March", "April", "May", "June", 
        "July", "August", "September", "October", "November", "December"
    };
    
    /// <summary>
    /// Returns the string name of a month
    /// </summary>
    /// <param name="index">1 - 12 inclusive</param>
    /// <returns>the name of the month</returns>
    public static string GetMonthFromIndex(int index) => _monthNames[index - 1];

    /// <summary>
    /// Gets the week number from the given date
    /// </summary>
    /// <param name="day"></param>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public static int GetWeekNumberFrom(int day, int month, int year)
    {
        DateTime date = new DateTime(year, month, day);
        CultureInfo ciCurr = CultureInfo.InvariantCulture;
        int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        return weekNum;
    }
}
