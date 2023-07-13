namespace Polarity.FluentApi.InfluxDB.Interfaces;

public interface ISortedFluxQuery : IBuildableFluxQuery
{
    IBuildableFluxQuery Limit(int limit);
}