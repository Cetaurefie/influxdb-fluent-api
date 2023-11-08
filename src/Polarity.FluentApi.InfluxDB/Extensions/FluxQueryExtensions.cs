using System;
using Polarity.FluentApi.InfluxDB.Interfaces;

namespace Polarity.FluentApi.InfluxDB.Extensions;

/// <summary>
/// Extensions methods for <see cref="IFluxQuery"/>
/// </summary>
public static class FluxQueryExtensions
{
    /// <summary>
    /// Append a Flux filter to only include a specified measurement
    /// <para>This is equivalent to the Flux <code>|> filter(fn: (r) => r._measurement == "{measurement}")</code></para>
    /// </summary>
    /// <param name="fluxQuery">The query to add a measurement filter</param>
    /// <param name="measurement">The name of the measurement to include</param>
    /// <returns></returns>
    public static IFluxQuery FilterMeasurement(this IFluxQuery fluxQuery, string measurement)
    {
        return fluxQuery.AppendClause($"|> filter(fn: (r) => r._measurement == \"{measurement}\")");
    }

    /// <summary>
    /// Append a custom Flux filter
    /// <para>This wraps the expression in the Flux <code>|> filter(fn: (r) => {expression})</code></para>
    /// <example><code>fluxQuery.Filter("r._measurement == "myMeasurement" and r._value >= 42 and r._value &lt;= 100")</code></example>
    /// </summary>
    /// <param name="fluxQuery">The query to add a custom filter</param>
    /// <param name="expression">Conditional Flux expression</param>
    /// <returns></returns>
    public static IFluxQuery Filter(this IFluxQuery fluxQuery, string expression)
    {
        return fluxQuery.AppendClause($"|> filter(fn: (r) => {expression})");
    }

    /// <summary>
    /// Append a Flux filter to only include a rows where <paramref name="column"/> equals <paramref name="value"/>.
    /// Automatically wraps string-type values in double quotes.
    /// <para>This is equivalent to the Flux <code>|> filter(fn: (r) => r.{column} == {value})</code></para>
    /// </summary>
    /// <param name="fluxQuery">The query to add an inclusive filter</param>
    /// <param name="column">Column to filter by</param>
    /// <param name="value">Desired value of the column</param>
    /// <returns></returns>
    public static IFluxQuery Include(this IFluxQuery fluxQuery, string column, object value)
    {
        string valueString = value is string ? $"\"{value}\"" : value.ToString();

        return fluxQuery.AppendClause($"|> filter(fn: (r) => r.{column} == {valueString})");
    }

    /// <summary>
    /// Append a Flux filter to only include a rows where <paramref name="column"/> does not equal <paramref name="value"/>.
    /// Automatically wraps string-type values in double quotes.
    /// <para>This is equivalent to the Flux <code>|> filter(fn: (r) => r.{column} != {value})</code></para>
    /// </summary>
    /// <param name="fluxQuery">The query to add an exclusive filter</param>
    /// <param name="column">Column to filter by</param>
    /// <param name="value">Undesired value of the column</param>
    /// <returns></returns>
    public static IFluxQuery Exclude(this IFluxQuery fluxQuery, string column, object value)
    {
        string valueString = value is string ? $"\"{value}\"" : value.ToString();

        return fluxQuery.AppendClause($"|> filter(fn: (r) => r.{column} != {valueString})");
    }

    /// <summary>
    /// Append a Flux selector to only return distinct values for a given <paramref name="column"/>.
    /// </summary>
    /// <param name="fluxQuery">The query to add a distinct selector</param>
    /// <param name="column">The column to select</param>
    /// <returns></returns>
    public static IFluxQuery Distinct(this IFluxQuery fluxQuery, string column)
    {
        return fluxQuery.AppendClause($"|> distinct(column: \"{column}\")");
    }

    /// <summary>
    /// Append an aggregate clause to calculate the mean value for a column grouped into windows of time.
    /// </summary>
    /// <param name="fluxQuery">The query to add a mean aggregation</param>
    /// <param name="aggregateWindow">Time group window</param>
    /// <param name="column">Column to aggregate</param>
    /// <param name="shouldCreateEmpty">If empty results should be created for aggregate windows without data</param>
    /// <returns></returns>
    public static IFluxQuery Mean(this IFluxQuery fluxQuery, TimeSpan aggregateWindow, string column = "_value", bool shouldCreateEmpty = false)
    {
        string duration = aggregateWindow.ToFluxDuration();
        string createEmpty = shouldCreateEmpty.ToString().ToLower();

        return fluxQuery.AppendClause($"|> aggregateWindow(every: {duration}, fn: mean, column: \"{column}\", createEmpty: {createEmpty})");
    }

    /// <summary>
    /// Append an aggregate clause to calculate the sum value for a column grouped into windows of time.
    /// </summary>
    /// <param name="fluxQuery">The query to add a mean aggregation</param>
    /// <param name="aggregateWindow">Time group window</param>
    /// <param name="column">Column to aggregate</param>
    /// <param name="shouldCreateEmpty">If empty results should be created for aggregate windows without data</param>
    /// <returns></returns>
    public static IFluxQuery Sum(this IFluxQuery fluxQuery, TimeSpan aggregateWindow, string column = "_value", bool shouldCreateEmpty = false)
    {
        string duration = aggregateWindow.ToFluxDuration();
        string createEmpty = shouldCreateEmpty.ToString().ToLower();

        return fluxQuery.AppendClause($"|> aggregateWindow(every: {duration}, fn: sum, column: \"{column}\", createEmpty: {createEmpty})");
    }

    /// <summary>
    /// Append an aggregate clause to calculate the mean value for a column grouped into windows of time.
    /// </summary>
    /// <param name="fluxQuery">The query to add a mean aggregation</param>
    /// <param name="aggregateWindow">Time group window</param>
    /// <param name="column">Column to aggregate</param>
    /// <param name="shouldCreateEmpty">If empty results should be created for aggregate windows without data</param>
    /// <returns></returns>
    public static IFluxQuery Quantile(this IFluxQuery fluxQuery, TimeSpan aggregateWindow, float quantile, string column = "_value", bool shouldCreateEmpty = false)
    {
        if (quantile > 1f || quantile < 0f)
            throw new ArgumentOutOfRangeException(nameof(quantile), "Quantile must be between 0f and 1f inclusive");

        string duration = aggregateWindow.ToFluxDuration();
        string createEmpty = shouldCreateEmpty.ToString().ToLower();
        string quantileFunction = $"(column, tables=<-) => tables |> quantile(q: {quantile}, column: column)";
        string aggregate = $"|> aggregateWindow(every: {duration}, column: \"{column}\", fn: {quantileFunction}, createEmpty: {createEmpty})";

        return fluxQuery.AppendClause(aggregate);
    }

    /// <summary>
    /// Append a Flux sort by the specified <paramref name="columns"/> in ascending order.
    /// <para>This is equivalent to the Flux <code>|> sort(columns: ["columns[0]", "columns[1]", ...], desc: false)</code></para>
    /// </summary>
    /// <param name="fluxQuery">The query to add a sort</param>
    /// <param name="columns">Columns to sort</param>
    /// <returns></returns>
    public static ISortedFluxQuery Sort(this IFluxQuery fluxQuery, params string[] columns)
    {
        return fluxQuery.Sort(false, columns);
    }

    /// <summary>
    /// Append a Flux sort by the specified <paramref name="columns"/> in descending order.
    /// <para>This is equivalent to the Flux <code>|> sort(columns: ["columns[0]", "columns[1]", ...], desc: true)</code></para>
    /// </summary>
    /// <param name="fluxQuery">The query to add a sort</param>
    /// <param name="columns">Columns to sort</param>
    /// <returns></returns>
    public static ISortedFluxQuery SortDescending(this IFluxQuery fluxQuery, params string[] columns)
    {
        return fluxQuery.Sort(true, columns);
    }

}
