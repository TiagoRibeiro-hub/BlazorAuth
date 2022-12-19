using AutoMapper.Mappers;
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

    public static string ToStringDate(this DateTime date) => $"{date.Year}-{(date.Month.ToString().Length == 1 ? $"0{date.Month}" : date.Month)}-{(date.Day.ToString().Length == 1 ? $"0{date.Day}" : date.Day)}";
}