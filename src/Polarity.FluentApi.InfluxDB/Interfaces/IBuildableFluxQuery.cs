namespace Polarity.FluentApi.InfluxDB.Interfaces;

public interface IBuildableFluxQuery
{
    /// <summary>
    /// Builds Flux clauses into a single formatted string
    /// </summary>
    /// <returns>Flux query string</returns>
    string Build();
}