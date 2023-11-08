using System;
using System.Text;

namespace Polarity.FluentApi.InfluxDB.Extensions;

internal static class TimeSpanExtensions
{
    /// <summary>
    /// Converts a TimeSpan into a FLux Duration.
    /// </summary>
    /// <param name="timeSpan"></param>
    /// <example>TimeSpan.FromDays(3) would return "3d"</example>
    /// <returns>0s if timeSpan is zero, otherwise the equivalent Flux Duration</returns>
    public static string ToFluxDuration(this TimeSpan timeSpan)
    {
        if (timeSpan == TimeSpan.Zero)
            return "0s";

        StringBuilder builder = new();

        if (timeSpan.Days > 0)
            builder.Append($"{timeSpan.Days}d");

        if (timeSpan.Hours > 0)
            builder.Append($"{timeSpan.Hours}h");

        if (timeSpan.Minutes > 0)
            builder.Append($"{timeSpan.Minutes}m");

        if (timeSpan.Seconds > 0)
            builder.Append($"{timeSpan.Seconds}s");

        if (timeSpan.Milliseconds > 0)
            builder.Append($"{timeSpan.Milliseconds}ms");

        string duration = builder.ToString();

        return duration;
    }
}
