
using System;

namespace Polarity.FluentApi.InfluxDB.Interfaces;

public interface IFluxQueryBuilder
{
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
    /// <returns></returns>
    IFluxQuery CreateQuery(string bucket, DateTime? start, DateTime? end);
}