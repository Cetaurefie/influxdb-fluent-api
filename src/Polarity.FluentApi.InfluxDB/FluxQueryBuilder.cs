using System;
using Polarity.FluentApi.InfluxDB.Interfaces;

namespace Polarity.FluentApi.InfluxDB;

public class FluxQueryBuilder : IFluxQueryBuilder
{
    /// <inheritdoc />
    public IFluxQuery CreateQuery(string bucket, DateTime? start, DateTime? end)
    {
        DateTime endTime = end ?? DateTime.UtcNow;
        long unixStart = start.HasValue ? new DateTimeOffset(start.Value).ToUnixTimeSeconds() : 0;
        long unixEnd = new DateTimeOffset(endTime).ToUnixTimeSeconds();

        FluxQuery query = new();

        query.AppendClause($"from(bucket: \"{bucket}\")");
        query.AppendClause($"|> range(start: {unixStart}, stop: {unixEnd})");

        return query;
    }
}