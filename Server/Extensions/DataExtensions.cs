using System.Globalization;

namespace BlazorAuth.Server.Extensions;

public static class DataExtensions
{
    public static bool TryDateParse(this string value, out DateTime? date)
    {
        date = null;
        if (!string.IsNullOrWhiteSpace(value) && DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            date = result;
            return true;
        }

        return false;
    }

    public static bool TryDateTimeParse(this string value, out DateTime? date)
    {
        date = null;
        var formats = new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss.ttt" };

        if (!string.IsNullOrWhiteSpace(value))
        {
            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    date = result;
                    return true;
                }
            }
        }

        return false;
    }

    public static bool AgeIsValid(this DateTime? date, int valueToCheck)
    {
        if (!date.HasValue || date.Value == default)
        {
            return false;
        }

        var ageInDays = (DateTime.Today - date.GetValueOrDefault()).TotalDays;
        var minimumAgeInDays = (DateTime.Today - DateTime.Today.AddYears(-valueToCheck)).TotalDays;

        return ageInDays >= minimumAgeInDays;
    }
}