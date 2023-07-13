using System;
using Polarity.FluentApi.InfluxDB.Interfaces;

namespace Polarity.FluentApi.InfluxDB;

public class FluxQueryBuilder : IFluxQueryBuilder
{
    public IFluxQuery CreateQuery(string bucket, DateTime? start, DateTime? end)
    {
        DateTime startTime = start ?? DateTime.MinValue;
        DateTime endTime = end ?? DateTime.UtcNow;
        long unixStart = new DateTimeOffset(startTime).ToUnixTimeSeconds();
        long unixEnd = new DateTimeOffset(endTime).ToUnixTimeSeconds();

        FluxQuery query = new();

        query.AppendClause($"from(bucket: \"{bucket}\")");
        query.AppendClause($"|> range(start: {unixStart}, stop: {unixEnd})");

        return query;
    }
}