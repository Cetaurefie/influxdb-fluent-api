namespace Polarity.FluentApi.InfluxDB.Interfaces;

/// <summary>
/// Sorted Flux query
/// </summary>
public interface ISortedFluxQuery : IBuildableFluxQuery
{
    /// <summary>
    /// Limits the number of rows returned by the Flux query. This is equivalent to
    /// <code>|> limit(n:{limit})</code>
    /// </summary>
    /// <param name="limit">Maximum number of rows to return</param>
    /// <returns></returns>
    IBuildableFluxQuery Limit(uint limit);
}