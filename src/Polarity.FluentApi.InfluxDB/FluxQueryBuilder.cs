using System;
using Polarity.FluentApi.InfluxDB.Interfaces;

namespace Polarity.FluentApi.InfluxDB;

/// <summary>
///
/// </summary>
public class FluxQueryBuilder : IFluxQueryBuilder
{
    /// <inheritdoc />
    public IFluxQuery CreateQuery(string bucket, DateTime? start = null, DateTime? end = null)
    {
        DateTime endTime = end ?? DateTime.UtcNow;
        long unixStart = start.HasValue ? new DateTimeOffset(start.Value).ToUnixTimeSeconds() : 0;
        long unixEnd = new DateTimeOffset(endTime).ToUnixTimeSeconds();

        FluxQuery query = new();

        query.AppendClause($"from(bucket: \"{bucket}\")");
        query.AppendClause($"|> range(start: {unixStart}, stop: {unixEnd})");

        return query;
    }

    /// <summary>
    /// Creates a new <see cref="IFluxQuery"/> using the specified bucket and time range. This is equivalent to the Flux:
    /// <code>
    /// from(bucket: "{bucket}")
    /// |> range(start: {start}, stop: {end})
    /// </code>
    /// </summary>
    /// <param name="bucket">InfluxDB bucket to query</param>
    /// <param name="start">Inclusive query start timestamp. Automatically coverts to UTC. 0 if NULL</param>
    /// <param name="end">Inclusive query end timestamp. Automatically converts to UTC. <see cref="DateTime.UtcNow"/> if NULL</param>
    public static IFluxQuery Create(string bucket, DateTime? start = null, DateTime? end = null) =>
        new FluxQueryBuilder().CreateQuery(bucket, start, end);
}