namespace Polarity.FluentApi.InfluxDB.Interfaces;

/// <summary>
/// Outputs a built Flux query
/// </summary>
public interface IBuildableFluxQuery
{
    /// <summary>
    /// Builds Flux clauses into a single formatted string
    /// </summary>
    /// <returns>Flux query string</returns>
    string Build();
}