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
}
